using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class AppDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.Role).IsRequired().HasMaxLength(20);
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Token).IsRequired();
                entity.Property(r => r.ExpiryDate).IsRequired();
                entity.HasOne(r => r.User)
                      .WithMany()
                      .HasForeignKey(r => r.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.Property(b => b.ISBN)
                      .IsRequired()
                      .HasMaxLength(13);
                entity.Property(b => b.Title).IsRequired();
                entity.Property(b => b.Genre).IsRequired();
                entity.Property(b => b.Description).IsRequired();
                entity.Property(b => b.BorrowedTime).IsRequired();
                entity.Property(b => b.ReturnTime).IsRequired();

                entity.HasOne(b => b.Author)
                      .WithMany()
                      .HasForeignKey(b => b.AuthorId);

                entity.HasOne(b => b.Borrower) 
                      .WithMany()
                      .HasForeignKey(b => b.BorrowerId)
                      .OnDelete(DeleteBehavior.SetNull); 
            });

            modelBuilder.Entity<Author>(entity =>
            {
                entity.HasKey(a => a.Id);
            });

            base.OnModelCreating(modelBuilder);
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
