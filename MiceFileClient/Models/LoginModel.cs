using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace MiceFileClient.Models
{
	public class LoginModel : INotifyPropertyChanged
	{
		private string email;
		public string Email { get => email; set { email = value; OnPropertyChanged(nameof(Email)); } }

		private string password;
		public string Password { get => password; set { password = value; OnPropertyChanged(nameof(Password)); } }


		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		}
	}
}
