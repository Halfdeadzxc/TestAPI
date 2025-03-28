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
                modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

                base.OnModelCreating(modelBuilder);
            }

            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
            {
            }
        }
    }
