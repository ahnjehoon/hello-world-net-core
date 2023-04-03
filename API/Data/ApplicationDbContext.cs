using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
	public partial class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext() { }

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		public virtual DbSet<Board> Board { get; set; } = null!;

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Board>(entity =>
			{
				entity.ToTable("board");

				entity.Property(e => e.Id).HasColumnName("id");

				entity.Property(e => e.Content)
					.HasMaxLength(200)
					.HasColumnName("content");

				entity.Property(e => e.Register)
					.HasMaxLength(50)
					.HasColumnName("register");

				entity.Property(e => e.RegisterDate).HasColumnName("register_date");

				entity.Property(e => e.Title)
					.HasMaxLength(50)
					.HasColumnName("title");
			});

			OnModelCreatingPartial(modelBuilder);
		}

		partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
	}
}
