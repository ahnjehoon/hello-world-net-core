namespace API.Dto
{
	public class CommentCreateRequest
	{
		public int BoardId { get; set; }
		public string Content { get; set; } = null!;
		public string Register { get; set; } = null!;
	}
}
