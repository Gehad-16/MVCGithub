using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Models
{
	public class RegisterViewModel
	{
		[Required(ErrorMessage ="FName is required")]
		public string FName { get; set; }
		[Required(ErrorMessage = "LName is required")]
		public string LName { get; set; }
		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage ="Invalid Email Address")]
		public string Email { get; set; }
		[Required(ErrorMessage = "Password is required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Required(ErrorMessage = "ConfirmPassword is required")]
		[DataType(DataType.Password)]
		[Compare("Password" , ErrorMessage ="Password Dosen't Match")]
		public string ConfirmPassword { get; set; }
        public bool IsAgree { get; set; }

    }
}
