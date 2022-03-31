using Microsoft.AspNetCore.Http;
using System;
using MiceFileClient.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiceFileClient.Models;
using MiceFileClient.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace MiceFileClient.Middleware
{
	public class ActiveUserMiddleware
	{
		private readonly RequestDelegate _next;

		public ActiveUserMiddleware(RequestDelegate next)
		{
			this._next = next;
		}

		public async Task InvokeAsync(HttpContext context, ApplicatinContext db)
		{
			string userSessionKey = "currentUser";
			if(context.User.Identity.IsAuthenticated 
				&& !context.Session.Keys.Contains(userSessionKey)) // user is authenticated, but usersession is not set
			{
				UserSession userSession = await db.Users.FirstOrDefaultAsync(u => u.Email == context.User.Identity.Name);
				context.Session.Set<UserSession>(userSessionKey, userSession);
			}
			else if(!context.User.Identity.IsAuthenticated
				&& context.Session.Keys.Contains(userSessionKey)) // user is not authenticated and usersession exists
			{
				context.Session.Remove(userSessionKey);
			}
			else if(context.User.Identity.IsAuthenticated 
				&& context.Session.Keys.Contains(userSessionKey)
				&& context.Session.Get<UserSession>(userSessionKey).Email != context.User.Identity.Name) // user is authenticated, but usersession is not correct (in case of changing login or username)
			{
				context.Session.Remove(userSessionKey);
				UserSession currentUser = await db.Users.FirstOrDefaultAsync(u => u.Email == context.User.Identity.Name);
				context.Session.Set<UserSession>(userSessionKey, currentUser);
			}
			await _next.Invoke(context);
		}
	}
}
