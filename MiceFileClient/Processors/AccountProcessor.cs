using MiceFileClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MiceFileClient.Processors
{
	class AccountProcessor
	{
		// TODO: Refactor boilerplate
		private static readonly string controller = "account";
		private static readonly string baseUrl = Properties.Settings.Default.HostUrl;
		public static async Task<string> LogIn(LoginModel model)
		{
			string action = "login";
			string url = $"{baseUrl}/{controller}/{action}/";
			if (model == null)
				throw new ArgumentNullException(nameof(model));
			if (model.Email == null)
				throw new ArgumentException("Не указан email");
			if (model.Password == null)
				throw new ArgumentException("Не указан пароль");

			using (HttpResponseMessage response = await ApiHelper.ApiClient.PostAsJsonAsync(url, model))
			{
				if (response.IsSuccessStatusCode)
				{
					return await response.Content.ReadAsStringAsync();
				}
				else
				{
					throw new Exception(await response.Content.ReadAsStringAsync());
				}
			}
		}
		public static async Task LogOut()
		{
			string action = "logout";
			string url = $"{baseUrl}/{controller}/{action}/";

			using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
			{
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsStringAsync();
				}
				else
				{
					throw new Exception(response.ReasonPhrase);
				}
			}
		}
		public static async Task<string> Register(RegisterModel model)
		{
			string action = "register";
			string url = $"{baseUrl}/{controller}/{action}/";

			if (model == null)
				throw new ArgumentNullException(nameof(model));
			if (model.Email == null)
				throw new ArgumentException("Не указан email");
			if (model.Password == null)
				throw new ArgumentException("Не указан пароль");

			if (model.Password != model.ConfirmPassword)
				throw new ArgumentException("Пароли не совпадают");

			using (HttpResponseMessage response = await ApiHelper.ApiClient.PostAsJsonAsync(url, model))
			{
				if (response.IsSuccessStatusCode)
				{
					return await response.Content.ReadAsStringAsync();
				}
				else
				{
					throw new Exception(await response.Content.ReadAsStringAsync());
				}
			}
		}
	}
}
