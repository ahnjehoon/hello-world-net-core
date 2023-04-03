using API.Entities;

namespace API.SeedData
{
	public class SeedMember
	{
		public List<Member> GetSeed()
		{
			return new List<Member>
			{
				new Member {Account = "account01", Password = "secret", Name = "유저1"},
				new Member {Account = "account02", Password = "secret", Name = "유저2"},
				new Member {Account = "account03", Password = "secret", Name = "유저3"}
			};
		}
	}
}
