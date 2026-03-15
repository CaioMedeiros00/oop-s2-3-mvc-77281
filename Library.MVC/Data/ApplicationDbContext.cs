using Library.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Library.MVC.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Loan> Loans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Book - Loan relationship
            modelBuilder.Entity<Loan>()
                .HasOne(l => l.Book)
                .WithMany(b => b.Loans)
                .HasForeignKey(l => l.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            // Member - Loan relationship
            modelBuilder.Entity<Loan>()
                .HasOne(l => l.Member)
                .WithMany(m => m.Loans)
                .HasForeignKey(l => l.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            // Book entity configuration
            modelBuilder.Entity<Book>()
                .Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<Book>()
                .Property(b => b.Author)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Book>()
                .Property(b => b.Isbn)
                .IsRequired()
                .HasMaxLength(20);

            modelBuilder.Entity<Book>()
                .Property(b => b.Category)
                .IsRequired()
                .HasMaxLength(50);

            // Member entity configuration
            modelBuilder.Entity<Member>()
                .Property(m => m.FullName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Member>()
                .Property(m => m.Email)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Member>()
                .Property(m => m.Phone)
                .IsRequired()
                .HasMaxLength(20);
        }
    }
}

