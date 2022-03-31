using MiceFileClient.Models;
using Microsoft.EntityFrameworkCore;

namespace MiceFileClient.DataAccess
{
	public class ApplicatinContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<DBFile> Files { get; set; }
		public ApplicatinContext(DbContextOptions<ApplicatinContext> options) : base(options)
		{
			Database.EnsureCreated();
		}		
	}
}
