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
	class RegisterViewModel : INotifyPropertyChanged
	{
		private RegisterModel credentials = new RegisterModel();
		public RegisterModel Credentials { get { return credentials; } set { credentials = value; OnPropertyChanged(nameof(Credentials)); } }

		private string message;
		public string Message { get { return message; } set { message = value; OnPropertyChanged(nameof(Message)); } }

		private bool registerButtonEnabled = true;
		public bool RegisterButtonEnabled { get { return registerButtonEnabled; } set { registerButtonEnabled = value; OnPropertyChanged(nameof(RegisterButtonEnabled)); } }

		private RelayCommand registerCommand;
		public RelayCommand RegisterCommand => registerCommand ??
			(registerCommand = new RelayCommand(async obj =>
			{
				Window window = obj as Window;
				try
				{
					Message = "Регистрация...";
					RegisterButtonEnabled = false;
					await AccountProcessor.Register(Credentials);
					LoginWindow loginWindow = new LoginWindow();
					window.Close();
					loginWindow.Show();
				}
				catch (Exception e)
				{
					RegisterButtonEnabled = true;
					Message = e.Message;
				}
			}));
		private RelayCommand logInWindowCommand;
		public RelayCommand LogInWindowCommand => logInWindowCommand ??
			(logInWindowCommand = new RelayCommand(obj =>
			{
				Window window = obj as Window;
				LoginWindow logInWindow = new LoginWindow();
				window.Close();
				logInWindow.Show();
			}));

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		}
	}
}
