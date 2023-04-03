using API.Data;
using API.Dto;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class CommentController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public CommentController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<IActionResult> Select()
		{
			var result = await _context.Comment
				.Include(t => t.Board)
				.Include(t => t.RegisterNavigation)
				.AsNoTracking()
				.ToListAsync();
			return Ok(result);
		}

		[HttpGet("{boardId}")]
		public async Task<IActionResult> SelectByBoard(int boardId)
		{
			var result = await _context.Comment.AsNoTracking().Where(c => c.BoardId == boardId).ToListAsync();
			return Ok(result);
		}

		[HttpGet("ASD")]
		public async Task<IActionResult> SelectByMember(string register)
		{
			var result = await _context.Comment.AsNoTracking().Where(c => true
				&& (c.Register == register)
			).ToListAsync();
			return Ok();
		}

		[HttpPost]
		public async Task<IActionResult> Create(CommentCreateRequest param)
		{
			var board = _context.Board.FirstOrDefault(c => c.Id == param.BoardId);
			if (board == null) return StatusCode(400, "NO BOARD");
			var member = _context.Member.FirstOrDefault(c => c.Account == param.Register);
			if (member == null) return StatusCode(400, "NO MEMBER");
			var newComment = new Comment
			{
				BoardId = param.BoardId,
				Content = param.Content,
				Register = param.Register,
				RegisterDate = DateTime.Now,
				Board = board,
				RegisterNavigation = member
			};
			await _context.Comment.AddAsync(newComment);
			await _context.SaveChangesAsync();
			return Ok();
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> CreateV1(Comment param)
		{
			await _context.Comment.AddAsync(param);
			await _context.SaveChangesAsync();
			return Ok();
		}

		[HttpPut]
		public async Task<IActionResult> Update(Comment param)
		{
			var temp = await _context.Comment.FirstOrDefaultAsync(c => c.Id == param.Id);
			if (temp == null) return StatusCode(400, "NO COMMENT");
			_context.Entry(temp).CurrentValues.SetValues(param);
			await _context.SaveChangesAsync();
			return Ok();
		}

		[HttpDelete]
		public async Task<IActionResult> Delete(int id)
		{
			var q = _context.Comment.Where(c => c.Id == id);
			_context.Comment.RemoveRange(q);
			await _context.SaveChangesAsync();
			return Ok();
		}
	}
}
