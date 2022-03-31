using MiceFileClient.Middleware;
using Microsoft.AspNetCore.Builder;

namespace MiceFileClient.Extensions
{
	public static class ActiveUserExtensions
	{
		public static IApplicationBuilder UseActiveUser(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<ActiveUserMiddleware>();
		}
	}
}
