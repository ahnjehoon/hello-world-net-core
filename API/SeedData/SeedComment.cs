using API.Entities;

namespace API.SeedData
{
	public class SeedComment
	{
		public List<Comment> GetData()
		{
			return new List<Comment>
			{
				new Comment {BoardId = 1, Content = "사용자2가 작성한 댓글", Register = "account02"},
				new Comment {BoardId = 1, Content = "사용자3가 작성한 댓글", Register = "account03"},
				new Comment {BoardId = 1, Content = "사용자2가 작성한 두번째 댓글", Register = "account02"},
			};
		}
	}
}
