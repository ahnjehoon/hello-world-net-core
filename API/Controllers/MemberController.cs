using API.Dto;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	[Authorize]
	public class MemberController : ControllerBase
	{
		private readonly MemberService _memberService;

		public MemberController(MemberService memberService)
		{
			_memberService = memberService;
		}

		[HttpGet]
		public async Task<IActionResult> Select([FromQuery] MemberSelectRequest param)
		{
			var result = await _memberService.Select(param);
			return Ok(result);
		}

		[HttpGet("{account}")]
		public async Task<IActionResult> Get(string account)
		{
			var result = await _memberService.FindOne(account);
			return Ok(result);
		}

		[HttpPost]
		public async Task<IActionResult> Create(MemberCreateRequest param)
		{
			await _memberService.Create(param);
			return Ok();
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> CreateV2(MemberCreateRequestV2 param)
		{
			await _memberService.CreateV2(param);
			return Ok();
		}

		[HttpPut("{account}")]
		public async Task<IActionResult> Update(string account, MemberUpdateRequest param)
		{
			await _memberService.Update(account, param);
			return Ok();
		}

		[HttpDelete("{account}")]
		public async Task<IActionResult> Delete(string account)
		{
			await _memberService.Delete(account);
			return Ok();
		}
	}
}
