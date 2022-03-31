using MiceFileClient.DataAccess;
using MiceFileClient.Extensions;
using MiceFileClient.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MiceFileClient.Controllers
{
	[Authorize]
	[Route("[controller]")]
	[ApiController]
	public class FilesController : ControllerBase
	{
		private ApplicatinContext db;
		public FilesController(ApplicatinContext context)
		{
			db = context;
		}
		[HttpGet]
		public IActionResult Get()
		{
			var files = db.Files
				.Where(f => f.OwnerId == HttpContext.Session.Get<UserSession>("currentUser").Id)
				.Select(f => new { Id = f.Id, Name = f.Name, FileSize = f.FileSize })
				.AsNoTracking().ToList();

			return new ObjectResult(files);
		}
		[HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			var file = db.Files
				.Where(f => f.OwnerId == HttpContext.Session.Get<UserSession>("currentUser").Id && f.Id == id)
				.Select(f => new { Name = f.Name, FileData = f.FileData })
				.AsNoTracking().FirstOrDefault();
			if (file == null)
			{
				return BadRequest("File not found");
			}
			return File(file.FileData, System.Net.Mime.MediaTypeNames.Application.Octet, file.Name);
		}
		[HttpPost]
		[Route("upload")]
		public async Task<IActionResult> AddFile([FromForm] UploadModel model)
		{
			DBFile file = model;
			file.OwnerId = HttpContext.Session.Get<UserSession>("currentUser").Id;

			await db.Files.AddAsync(file);
			await db.SaveChangesAsync();

			return new ObjectResult("Successfully added file.");
		}
		[HttpPost]
		[Route("remove")]
		public async Task<IActionResult> RemoveFile([FromBody] int id)
		{
			var fileToDelete = await db.Files.Where(f => f.OwnerId == HttpContext.Session.Get<UserSession>("currentUser").Id && f.Id == id).FirstOrDefaultAsync();
			if(fileToDelete == null)
			{
				return new ObjectResult("File not found.");
			}
			db.Files.Remove(fileToDelete);
			await db.SaveChangesAsync();
			return new ObjectResult("Successfully removed file.");
		}
	}
}
