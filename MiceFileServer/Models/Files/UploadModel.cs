using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MiceFileClient.Models
{
	public class UploadModel
	{
		[Required(ErrorMessage = "Не указано название файла")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Файл не предоставлен")]
		[DataType(DataType.Upload)]
		public IFormFile File { get; set; }
	}
}
