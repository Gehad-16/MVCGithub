﻿using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Models
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Invalid Email Address")]
		public string Email { get; set; }
		[Required(ErrorMessage = "Password is required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		public bool RemeberMe { get; set; }
	}
}
