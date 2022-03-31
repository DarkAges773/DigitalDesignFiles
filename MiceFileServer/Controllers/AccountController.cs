using MiceFileClient.DataAccess;
using MiceFileClient.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace MiceFileClient.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private ApplicatinContext db;
		public AccountController(ApplicatinContext context)
		{
			db = context;
		}
		[HttpGet]
		[Route("login")]
		public IActionResult Login()
		{
			return new ObjectResult("Please login or register new account");
		}
		[HttpPost]
		[Route("login")]
		public async Task<IActionResult> Login([FromBody] LoginModel model)
		{
			if (ModelState.IsValid)
			{
				User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
				if (user != null && user.Password == GeneratePasswordHash(user.Salt, model.Password))
				{
					await Authenticate(user); // authenticate

					return new ObjectResult("Successfully logged in.");
				}
				ModelState.AddModelError("Credentials", "Login or password is incorrect");
			}
			return BadRequest(ModelState);
		}
		[HttpPost]
		[Route("register")]
		public async Task<IActionResult> Register([FromBody] RegisterModel model)
		{
			if (ModelState.IsValid)
			{
				User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
				if (user == null)
				{
					// generate salt
					byte[] salt = new byte[128 / 8];
					using (var rngCsp = new RNGCryptoServiceProvider())
					{
						rngCsp.GetNonZeroBytes(salt);
					}
					// add user to db
					await db.Users.AddAsync(new User { Email = model.Email, Password = GeneratePasswordHash(salt, model.Password), Salt = salt });
					await db.SaveChangesAsync();

					return new ObjectResult("Successfully registered.");
				}
				else
					ModelState.AddModelError("Email", "User with this email already exists.");
			}
			return BadRequest(ModelState);
		}
		private static string GeneratePasswordHash(byte[] salt, string password)
		{
			string passwordHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
				password: password,
				salt: salt,
				prf: KeyDerivationPrf.HMACSHA256,
				iterationCount: 100000,
				numBytesRequested: 256 / 8));
			return passwordHash;
		}
		private async Task Authenticate(User user)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email)
			};
			ClaimsIdentity identity = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
			// установка аутентификационных куки
			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
			if (!User.Identity.IsAuthenticated) Unauthorized();
		}
		[HttpGet]
		[Authorize]
		[Route("logout")]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return new ObjectResult("Successfully logged out.");
		}
		[HttpGet]
		[Authorize]
		[Route("get_current_user")]
		public IActionResult TestUser()
		{
			return new ObjectResult(User.Identity.Name);
		}
	}
}
