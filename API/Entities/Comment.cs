namespace API.Entities
{
	public partial class Comment
	{
		public int BoardId { get; set; }
		public int Id { get; set; }
		public string Content { get; set; } = null!;
		public string Register { get; set; } = null!;
		public DateTime RegisterDate { get; set; }

		public virtual Board Board { get; set; } = null!;
		public virtual Member RegisterNavigation { get; set; } = null!;
	}
}
