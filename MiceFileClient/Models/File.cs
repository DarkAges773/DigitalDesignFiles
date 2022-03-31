using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace MiceFileClient.Models
{
	public class File : INotifyPropertyChanged
	{
		private int id;
		public int Id { get => id; set { id = value; OnPropertyChanged(nameof(Id)); } }

		private string name;
		public string Name { get => name; set { name = value; OnPropertyChanged(nameof(Name)); } }

		private long fileSize;
		public long FileSize { get => fileSize; set { fileSize = value; OnPropertyChanged(nameof(FileSize)); } }

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		}
	}
}
