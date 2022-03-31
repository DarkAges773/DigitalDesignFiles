using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MiceFileClient
{
	public static class ApiHelper
	{
		public static HttpClient ApiClient { get; set; }

		public static void InitializeClient()
		{
			HttpClientHandler clientHandler = new HttpClientHandler();
			clientHandler.UseCookies = true;
			clientHandler.CookieContainer = new CookieContainer();

			ApiClient = new HttpClient(clientHandler);
			ApiClient.DefaultRequestHeaders.Clear();
			ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}
	}
}
