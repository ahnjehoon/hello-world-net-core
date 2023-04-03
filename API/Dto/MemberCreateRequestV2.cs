namespace API.Dto
{
	public class MemberCreateRequestV2
	{
		public string Account { get; set; } = null!;
		public string Password { get; set; } = null!;
		public string? Name { get; set; }
		public List<BoardCreateProperty>? Board { get; set; }
	}
}
