namespace API.SeedData
{
	public class SeedBoard
	{
		public List<Board> GetData()
		{
			return new List<Board>
			{
				new Board {Title = "게시판-사용자1이 작성함", Content = "내용", Register = "account01"},
				new Board {Title = "게시판-사용자3이 작성함", Content = "내용", Register = "account03"}
			};
		}
	}
}
