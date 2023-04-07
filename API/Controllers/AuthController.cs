using API.Data;
using API.Dto;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
	[ApiController]
	[AllowAnonymous]
	[Route("[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly IConfiguration _configuration;

		public AuthController(ApplicationDbContext context, IConfiguration configuration)
		{
			_context = context;
			_configuration = configuration;
		}

		[HttpPost]
		public async Task<IActionResult> Login([FromBody] LoginRequest param)
		{
			var member = await _context.Member.AsNoTracking()
				.Where(c => c.Account == param.Account && c.Password == param.Password)
				.FirstOrDefaultAsync();
			if (member == null) return BadRequest("ID OR PASSOWRD NOT COLLECT");

			return Ok(CreateJwt(member));
		}

		private string CreateJwt(Member member)
		{
			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_configuration["JwtKey"]);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(nameof(Member.Account), member.Account)
				}),
				Expires = DateTime.Now.AddDays(1),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
			};
			SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		[HttpGet("[action]")]
		[Authorize]
		public async Task<IActionResult> RefreshToken()
		{
			// JWT에서 어떻게 값을 추출하는지를 위한 샘플
			var accessToken = Request.Headers[HeaderNames.Authorization].ToString()[7..];
			var jwt = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
			var account = jwt.Payload.GetValueOrDefault(nameof(Member.Account)).ToString();
			var member = await _context.Member.AsNoTracking().FirstOrDefaultAsync(c => c.Account == account);
			if (member == null) return BadRequest("ACCOUNT IS NOT EXIST");
			return Ok(CreateJwt(member));
		}
	}
}
