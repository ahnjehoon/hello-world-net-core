using API.Entities;

namespace API
{
	public partial class Board

		public int Id { get; set; }
		public string Title { get; set; } = null!;
		public string Content { get; set; } = null!;
		public string Register { get; set; } = null!;
		public DateTime RegisterDate { get; set; }
	}
}
