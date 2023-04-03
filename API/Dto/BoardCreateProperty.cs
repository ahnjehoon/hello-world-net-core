namespace API.Dto
{
	public class BoardCreateProperty
	{
		public string Title { get; set; } = null!;
		public string Content { get; set; } = null!;
		public string Register { get; set; } = null!;
		public List<CommentCreateProperty>? Comment { get; set; }
	}
}
