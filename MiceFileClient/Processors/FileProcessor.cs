using MiceFileClient.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MiceFileClient.Processors
{
	class FileProcessor
	{
		private static readonly string controller = "files";
		private static readonly string baseUrl = "https://localhost:44372";
		public static async Task SaveFile(string fileSaveDirectory, byte[] fileData, string fileName)
		{
			string filePath = $"{fileSaveDirectory}\\{fileName}";
			await System.IO.File.WriteAllBytesAsync(filePath, fileData);
		}
		public static async Task<byte[]> DownloadFile(int id)
		{
			string url = $"{baseUrl}/{controller}/{id}";

			using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
			{
				if (response.IsSuccessStatusCode)
				{
					return await response.Content.ReadAsByteArrayAsync();
				}
				else
				{
					throw new Exception(await response.Content.ReadAsStringAsync());
				}
			}
		}
		public static async Task<ObservableCollection<Models.File>> LoadFiles()
		{
			string url = $"{baseUrl}/{controller}/";

			using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
			{
				if (response.IsSuccessStatusCode)
				{
					return await response.Content.ReadAsAsync<ObservableCollection<Models.File>>();
				}
				else
				{
					throw new Exception(await response.Content.ReadAsStringAsync());
				}
			}
		}
		public static async Task Upload(string FilePath, string FileName)
		{
			string action = "upload";
			string url = $"{baseUrl}/{controller}/{action}";

			using var content = new MultipartFormDataContent();
			await using FileStream stream = System.IO.File.OpenRead(FilePath);

			content.Add(new StreamContent(stream), "file", FileName);
			content.Add(new StringContent(FileName), "name");
			using (HttpResponseMessage response = await ApiHelper.ApiClient.PostAsync(url, content))
			{
				if (response.IsSuccessStatusCode)
				{
					return;
				}
				else
				{
					throw new Exception(await response.Content.ReadAsStringAsync());
				}
			}
		}
		public static async Task DeleteFile(int id)
		{
			string action = "remove";
			string url = $"{baseUrl}/{controller}/{action}";

			using (HttpResponseMessage response = await ApiHelper.ApiClient.PostAsJsonAsync(url, id))
			{
				if (response.IsSuccessStatusCode)
				{
					return;
				}
				else
				{
					throw new Exception(await response.Content.ReadAsStringAsync());
				}
			}
		}
	}
}
