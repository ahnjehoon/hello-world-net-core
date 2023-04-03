using API.Data;
using API.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class BoardController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public BoardController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var result = await _context.Board
				.Include(t => t.RegisterNavigation)
				.Include(t => t.Comment)
				.AsNoTracking()
				.ToListAsync();
			return Ok(result);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			var result = await _context.Board.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
			return Ok(result);
		}

		[HttpPost]
		public async Task<IActionResult> Post(BoardCreateRequest param)
		{
			var member = _context.Member.FirstOrDefault(c => c.Account == param.Register);
			if (member == null) return StatusCode(400, "NO MEMBER");
			var newBoard = new Board
			{
				Title = param.Title,
				Content = param.Content,
				Register = param.Register,
				RegisterDate = DateTime.Now,
				RegisterNavigation = member
			};
			await _context.Board.AddAsync(newBoard);
			await _context.SaveChangesAsync();
			return Ok();
		}

		[HttpPut]
		public async Task<IActionResult> Put(Board param)
		{
			var temp = await _context.Board.FirstOrDefaultAsync(c => c.Id == param.Id);
			if (temp == null) return StatusCode(400, "NO BOARD");
			_context.Entry(temp).CurrentValues.SetValues(param);
			await _context.SaveChangesAsync();
			return Ok();
		}

		[HttpDelete]
		public async Task<IActionResult> Delete(int id)
		{
			var q = _context.Board.Where(c => c.Id == id);
			_context.Board.RemoveRange(q);
			await _context.SaveChangesAsync();
			return Ok();
		}
	}
}
