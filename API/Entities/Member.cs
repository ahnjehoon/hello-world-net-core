namespace API.Entities
{
	public partial class Member
	{
		public Member()
		{
			Board = new HashSet<Board>();
			Comment = new HashSet<Comment>();
		}

		public string Account { get; set; } = null!;
		public string Password { get; set; } = null!;
		public string? Name { get; set; }

		public virtual ICollection<Board> Board { get; set; }
		public virtual ICollection<Comment> Comment { get; set; }
	}
}
