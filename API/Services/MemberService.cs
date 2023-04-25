using API.Data;
using API.Dto;
using API.Entities;
using API.Utils;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace API.Services
{
	public class MemberService
	{
		private readonly ApplicationDbContext _context;
		private readonly IConfiguration _configuration;
		private readonly string _saltKey;

		public MemberService(ApplicationDbContext context, IConfiguration configuration)
		{
			_context = context;
			_configuration = configuration;
			_saltKey = _configuration[nameof(_saltKey)];
		}

		public async Task<Member?> Login(LoginRequest param)
		{
			param.Password = param.Password.EncryptSHA512(_saltKey);
			return await _context.Member.AsNoTracking()
				.Where(c => c.Account == param.Account && c.Password == param.Password)
				.FirstOrDefaultAsync();
		}

		public async Task<Member> FindOne(string param)
		{
			var result = await _context.Member
				.Include(t => t.Board)
				.Include(t => t.Comment)
			.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Account == param);
			return result;
		}

		public async Task<IEnumerable<Member>> Select(MemberSelectRequest param)
		{
			var result = await _context.Member
				.Where(c => true
					&& (param.Account == null || param.Account.Contains(c.Account))
					&& (param.LikeAccount == null || c.Account.Contains(param.LikeAccount))
					&& (param.LikeName == null || c.Name.Contains(param.LikeName))
				)
				.AsNoTracking()
				.ToListAsync();
			return result;
		}

		public async Task Create(MemberCreateRequest param)
		{
			var newMember = new Member
			{
				Account = param.Account,
				Password = param.Password,
				Name = param.Name
			};
			await Create(new[] { newMember });
		}

		public async Task Create(ICollection<Member> param)
		{
			foreach (var member in param)
			{
				var isDuplicated = await _context.Member.AnyAsync(c => c.Account == member.Account);
				if (isDuplicated) throw new Exception("MEMBER ALREADY EXIST");
				member.Password = member.Password.EncryptSHA512(_saltKey);
			}
			await _context.Member.AddRangeAsync(param);
			await _context.SaveChangesAsync();
		}

		public async Task CreateV2(MemberCreateRequestV2 param)
		{
			var isDuplicated = await _context.Member.AnyAsync(c => c.Account == param.Account);
			if (isDuplicated) throw new Exception("MEMBER ALREADY EXIST");
			var newMember = new Member
			{
				Account = param.Account,
				Password = param.Password.EncryptSHA512(_saltKey),
				Name = param.Name
			};
			param.Board?.ForEach(board =>
			{
				var boardRegister = _context.Member.FirstOrDefault(c => c.Account == board.Register);
				var newBoard = new Board
				{
					Title = board.Title,
					Content = board.Content,
					Register = board.Register,
					RegisterDate = DateTime.Now,
					RegisterNavigation = boardRegister ?? newMember
				};
				board.Comment?.ForEach(comment =>
				{
					var commentRegister = _context.Member.FirstOrDefault(c => c.Account == comment.Register);
					var newComment = new Comment
					{
						Content = comment.Content,
						Register = comment.Register,
						RegisterDate = DateTime.Now,
						Board = newBoard,
						RegisterNavigation = commentRegister ?? newMember
					};
					newBoard.Comment.Add(newComment);
				});
				newMember.Board.Add(newBoard);
			});
			await _context.Member.AddAsync(newMember);
			await _context.SaveChangesAsync();
		}

		public async Task Update(string param, MemberUpdateRequest param2)
		{
			var target = await _context.Member.FirstOrDefaultAsync(c => c.Account == param);
			if (target == null) throw new Exception("NO MEMBER");

			target.Name = param2.Name ?? target.Name;
			target.Password = param2.Password ?? target.Password;

			await _context.SaveChangesAsync();
		}

		public async Task Delete(string param)
		{
			var target = await _context.Member.FindAsync(param);
			if (target == null) throw new Exception("NO MEMBER");

			_context.Member.Remove(target);
			await _context.SaveChangesAsync();
		}
	}
}
