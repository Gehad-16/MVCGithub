using System.Collections;
using System.Collections.Generic;

namespace Demo.PL.Models
{
	public class UserViewModel
	{
		public string Id { get; set; }
		public string FName { get; set; }
		public string LName { get; set; }
		public string Email { get; set; }
		public string phoneNumber { get; set; }
		public IEnumerable<string> Roles { get; set; }
		
	}
}
