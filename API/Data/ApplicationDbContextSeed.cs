using API.Entities;
using API.Utils;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
	public class ApplicationDbContextSeed
	{
		private readonly ILogger<ApplicationDbContextSeed> _logger;
		private readonly IConfiguration _configuration;
		private readonly ApplicationDbContext _context;

		public ApplicationDbContextSeed(ILogger<ApplicationDbContextSeed> logger, IConfiguration configuration, ApplicationDbContext context)
		{
			_logger = logger;
			_configuration = configuration;
			_context = context;
		}

		public async Task SeedAsync(int retry = 0)
		{
			try
			{
				if (await _context.Member.AnyAsync() == false)
				{
					var tempData = SeedHelper.SeedData<Member>(nameof(Member) + ".json");
					if (tempData == null)
					{
						_logger.LogInformation("SEED FILE NOT EXIST OR NULL", nameof(Member));
						return;
					}
					await _context.Member.AddRangeAsync(tempData);
					_logger.LogInformation("SEED {TYPE} SUCCESS", nameof(Member));
				}

				if (await _context.Board.AnyAsync() == false)
				{
					var tempData = SeedHelper.SeedData<Board>(nameof(Board) + ".json");
					if (tempData == null)
					{
						_logger.LogInformation("SEED FILE NOT EXIST OR NULL", nameof(Board));
						return;
					}
					await _context.Board.AddRangeAsync(tempData);
					_logger.LogInformation("SEED {TYPE} SUCCESS", nameof(Board));
				}

				if (await _context.Comment.AnyAsync() == false)
				{
					var tempData = SeedHelper.SeedData<Comment>(nameof(Comment) + ".json");
					if (tempData == null)
					{
						_logger.LogInformation("SEED FILE NOT EXIST OR NULL", nameof(Comment));
						return;
					}
					await _context.Comment.AddRangeAsync(tempData);
					_logger.LogInformation("SEED {TYPE} SUCCESS", nameof(Comment));
				}
				await _context.SaveChangesAsync();
			}
			catch (Exception e)
			{
				if (retry < 5)
				{
					_logger.LogError("DB SEED RETRY COUNT: {RETRY}", retry);
					retry++;
					await SeedAsync(retry);
				}
				else
				{
					_logger.LogError("An error occurred {TYPE}. Exception: {@EXCEPTION}", nameof(ApplicationDbContextSeed), e);
				}
			}
		}
	}
}
