using System.IO;

namespace MiceFileClient.Models
{
	public class DBFile
	{
		public int Id { get; set; }
		public int OwnerId { get; set; }
		public User? Owner { get; set; }
		public string Name { get; set; }
		public long FileSize { get; set; }
		public byte[] FileData { get; set; }

		public static implicit operator DBFile(UploadModel v)
		{
			var file = new DBFile();
			using (BinaryReader binaryReader = new BinaryReader(v.File.OpenReadStream()))
			{
				file.FileData = binaryReader.ReadBytes((int)v.File.Length);
			}
			file.Name = v.Name;
			file.FileSize = v.File.Length;
			return file;
		}
	}
}
