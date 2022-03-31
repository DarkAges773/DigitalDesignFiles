using MiceFileClient.Models;
using MiceFileClient.Processors;
using Microsoft.Win32;
using Syroot.Windows.IO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MiceFileClient.ViewModels
{
	class MainViewModel : INotifyPropertyChanged
	{
		public MainViewModel()
		{
			ReloadFilesCommand.Execute(null);
		}

		private bool refreshButtonState;
		public bool RefreshButtonState { get => refreshButtonState; set { refreshButtonState = value; OnPropertyChanged(nameof(RefreshButtonState)); } }

		private bool uploadButtonState = true;
		public bool UploadButtonState { get => uploadButtonState; set { uploadButtonState = value; OnPropertyChanged(nameof(UploadButtonState)); } }

		private string fileUploadMessage;
		public string FileUploadMessage { get => fileUploadMessage; set { fileUploadMessage = value; OnPropertyChanged(nameof(FileUploadMessage)); } }

		private string fileName;
		public string FileName { get => fileName; set { fileName = value; OnPropertyChanged(nameof(FileName)); } }

		private string filePath;
		public string FilePath { get => filePath; set { filePath = value; OnPropertyChanged(nameof(FilePath)); } }

		private File selectedFile;
		public File SelectedFile { get => selectedFile; set { selectedFile = value; OnPropertyChanged(nameof(SelectedFile)); } }

		public ObservableCollection<File> files;
		public ObservableCollection<File> Files { get => files; set { files = value; OnPropertyChanged(nameof(Files)); } }

		private RelayCommand downloadFileCommand;
		public RelayCommand DownloadFileCommand => downloadFileCommand ??
			(downloadFileCommand = new RelayCommand(async obj =>
			{
				try
				{
					if (SelectedFile != null)
					{
						byte[] fileData = await FileProcessor.DownloadFile(SelectedFile.Id);
						await FileProcessor.SaveFile(new KnownFolder(KnownFolderType.Downloads).Path, fileData, SelectedFile.Name);
					}
				}
				catch (Exception e)
				{

				}
			}));

		private RelayCommand deleteFileCommand;
		public RelayCommand DeleteFileCommand => deleteFileCommand ??
			(deleteFileCommand = new RelayCommand(async obj =>
			{
				try
				{
					if (SelectedFile != null)
					{
						await FileProcessor.DeleteFile(SelectedFile.Id);
						ReloadFilesCommand.Execute(null);
					}
				}
				catch (Exception e)
				{

				}
			}));

		private RelayCommand chooseFileCommand;
		public RelayCommand ChooseFileCommand => chooseFileCommand ??
			(chooseFileCommand = new RelayCommand(obj =>
			{
				OpenFileDialog fileDialog = new OpenFileDialog();
				fileDialog.FileName = FilePath;
				if (fileDialog.ShowDialog() == true)
				{
					FilePath = fileDialog.FileName;
					FileName = fileDialog.SafeFileName;
				}
			}));

		private RelayCommand reloadFilesCommand;
		public RelayCommand ReloadFilesCommand => reloadFilesCommand ??
			(reloadFilesCommand = new RelayCommand(async obj =>
			{
				RefreshButtonState = false;
				try
				{
					Files = await FileProcessor.LoadFiles();
				}
				catch (Exception e)
				{

				}
				RefreshButtonState = true;
			}));

		private RelayCommand uploadFileCommand;
		public RelayCommand UploadFileCommand => uploadFileCommand ??
			(uploadFileCommand = new RelayCommand(async obj =>
			{
				UploadButtonState = false;
				try
				{
					await FileProcessor.Upload(FilePath, FileName);
					FileUploadMessage = "Файл успешно загружен";
					ReloadFilesCommand.Execute(null);
				}
				catch (Exception e)
				{
					FileUploadMessage = e.Message;
				}
				UploadButtonState = true;
			}));


		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		}
	}
}
