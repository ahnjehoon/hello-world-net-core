using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
	public partial class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext() { }

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		public virtual DbSet<Board> Board { get; set; } = null!;
		public virtual DbSet<Comment> Comment { get; set; } = null!;
		public virtual DbSet<Member> Member { get; set; } = null!;

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

				entity.HasOne(d => d.RegisterNavigation)
					.WithMany(p => p.Board)
					.HasForeignKey(d => d.Register)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_board_register_member_account");
			});

			modelBuilder.Entity<Comment>(entity =>
			{
				entity.HasKey(e => new { e.BoardId, e.Id })
					.HasName("PK__comment__board_id__id");

				entity.ToTable("comment");

				entity.HasComment("FK_comment_reg_id_users_id");

				entity.Property(e => e.BoardId).HasColumnName("board_id");

				entity.Property(e => e.Id)
					.ValueGeneratedOnAdd()
					.HasColumnName("id");

				entity.Property(e => e.Content)
					.HasMaxLength(200)
					.HasColumnName("content");

				entity.Property(e => e.Register)
					.HasMaxLength(50)
					.HasColumnName("register");

				entity.Property(e => e.RegisterDate).HasColumnName("register_date");

				entity.HasOne(d => d.Board)
					.WithMany(p => p.Comment)
					.HasForeignKey(d => d.BoardId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_comment_board_id_board_id");

				entity.HasOne(d => d.RegisterNavigation)
					.WithMany(p => p.Comment)
					.HasForeignKey(d => d.Register)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_comment_register_member_account");
			});

			modelBuilder.Entity<Member>(entity =>
			{
				entity.HasKey(e => e.Account)
					.HasName("PK__member__account");

				entity.ToTable("member");

				entity.Property(e => e.Account)
					.HasMaxLength(50)
					.HasColumnName("account");

				entity.Property(e => e.Name)
					.HasMaxLength(50)
					.HasColumnName("name");

				entity.Property(e => e.Password)
					.HasMaxLength(50)
					.HasColumnName("password");
			});

			OnModelCreatingPartial(modelBuilder);
		}

		partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
	}
}
