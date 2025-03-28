using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.ISBN).IsRequired().HasMaxLength(13);
            builder.Property(b => b.Title).IsRequired();
            builder.Property(b => b.Genre).IsRequired();
            builder.Property(b => b.Description).IsRequired();
            builder.Property(b => b.BorrowedTime).IsRequired().HasColumnType("timestamp");
            builder.Property(b => b.ReturnTime).IsRequired().HasColumnType("timestamp");

            builder.HasOne(b => b.Author)
                   .WithMany()
                   .HasForeignKey(b => b.AuthorId);

            builder.HasOne(b => b.Borrower)
                   .WithMany()
                   .HasForeignKey(b => b.BorrowerId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
