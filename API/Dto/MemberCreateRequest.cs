namespace API.Dto
{
	public class MemberCreateRequest
	{
		public string Account { get; set; } = null!;
		public string Password { get; set; } = null!;
		public string? Name { get; set; }
	}
}
