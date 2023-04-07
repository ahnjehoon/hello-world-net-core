using System.ComponentModel.DataAnnotations;

namespace API.Dto
{
	public class LoginRequest
	{
		[Required]
		public string Account { get; set; } = null!;
		[Required]
		public string Password { get; set; } = null!;
	}
}
