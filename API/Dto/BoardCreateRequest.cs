namespace API.Dto
{
	public class BoardCreateRequest
	{
		public string Title { get; set; } = null!;
		public string Content { get; set; } = null!;
		public string Register { get; set; } = null!;
	}
}
