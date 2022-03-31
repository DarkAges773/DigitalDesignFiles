using MiceFileClient.Models;
using MiceFileClient.Processors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MiceFileClient.ViewModels
{
	class LoginViewModel : INotifyPropertyChanged
	{
		private LoginModel credentials = new LoginModel();
		public LoginModel Credentials { get { return credentials; } set { credentials = value; OnPropertyChanged(nameof(Credentials)); } }

		private string message;
		public string Message { get { return message; } set { message = value; OnPropertyChanged(nameof(Message)); } }

		private bool logInButtonEnabled = true;
		public bool LogInButtonEnabled { get { return logInButtonEnabled; } set { logInButtonEnabled = value; OnPropertyChanged(nameof(LogInButtonEnabled)); } }

		private RelayCommand logInCommand;
		public RelayCommand LogInCommand => logInCommand ??
			(logInCommand = new RelayCommand(async obj =>
			{
				Window window = obj as Window;
				try
				{
					Message = "Аутентификация...";
					LogInButtonEnabled = false;
					await AccountProcessor.LogIn(Credentials);
					MainWindow mainWindow = new MainWindow();
					window.Close();
					mainWindow.Show();
				}
				catch (Exception e)
				{
					LogInButtonEnabled = true;
					Message = e.Message;
				}
			}));
		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		}

		private RelayCommand registerWindowCommand;
		public RelayCommand RegisterWindowCommand => registerWindowCommand ?? 
			(registerWindowCommand = new RelayCommand(obj =>
			{
				Window window = obj as Window;
				RegisterWindow registerWindow = new RegisterWindow();
				window.Close();
				registerWindow.Show();
			}));
	}
}
