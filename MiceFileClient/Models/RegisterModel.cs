using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MiceFileClient.Models
{
	public class RegisterModel : INotifyPropertyChanged
	{
		private string email;
		public string Email { get => email; set { email = value; OnPropertyChanged(nameof(Email)); } }

		private string password;
		public string Password { get => password; set { password = value; OnPropertyChanged(nameof(Password)); } }

		public string confirmPassword;
		public string ConfirmPassword { get => confirmPassword; set { confirmPassword = value; OnPropertyChanged(nameof(ConfirmPassword)); } }

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		}
	}
}
