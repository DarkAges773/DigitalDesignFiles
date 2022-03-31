using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiceFileClient.Models
{
	public class UserSession
	{
		public int Id { get; set; }
		public string Email { get; set; }

		public static implicit operator UserSession(User v)
		{
			return new UserSession() { Id = v.Id, Email = v.Email };
		}
	}
}
