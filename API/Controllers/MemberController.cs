using API.Data;
using API.Dto;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class MemberController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public MemberController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<IActionResult> Select([FromQuery] MemberSelectRequest param)
		{
			var result = await _context.Member
				.Where(c => true
					&& (param.Account == null || param.Account.Contains(c.Account))
					&& (param.LikeAccount == null || c.Account.Contains(param.LikeAccount))
					&& (param.LikeName == null || c.Name.Contains(param.LikeName))
				)
				.AsNoTracking()
				.Select(c => new
				{
					c.Account,
					c.Name
				})
				.ToListAsync();
			return Ok(result);
		}

		[HttpGet("{account}")]
		public async Task<IActionResult> Get(string account)
		{
			var result = await _context.Member
				.Include(t => t.Board)
				.Include(t => t.Comment)
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Account == account);
			return Ok(result);
		}

		[HttpPost]
		public async Task<IActionResult> Create(MemberCreateRequest param)
		{
			var isDuplicated = await _context.Member.AnyAsync(c => c.Account == param.Account);
			if (isDuplicated) return StatusCode(400, "MEMBER ALREADY EXIST");
			var newMember = new Member
			{
				Account = param.Account,
				Password = param.Password,
				Name = param.Name
			};
			await _context.Member.AddAsync(newMember);
			await _context.SaveChangesAsync();
			return Ok();
		}

		[HttpPut("{account}")]
		public async Task<IActionResult> Update(string account, MemberUpdateRequest param)
		{
			var target = await _context.Member.FirstOrDefaultAsync(c => c.Account == account);
			if (target == null) return StatusCode(400, "NO MEMBER");

			target.Name = param.Name ?? target.Name;
			target.Password = param.Password ?? target.Password;

			await _context.SaveChangesAsync();
			return Ok();
		}

		[HttpDelete("{account}")]
		public async Task<IActionResult> Delete(string account)
		{
			var target = await _context.Member.FindAsync(account);
			if (target == null) return StatusCode(400, "NO MEMBER");

			_context.Member.Remove(target);
			await _context.SaveChangesAsync();
			return Ok();
		}
	}
}
