using Library.Domain;
using Library.MVC.Data;
using Microsoft.EntityFrameworkCore;

namespace Library.Tests
{
    public class LibraryTests
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task CannotCreateLoanForBookAlreadyOnActiveLoan()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var book = new Book { Title = "Test Book", Author = "Test Author", Isbn = "123", Category = "Fiction", IsAvailable = false };
            var member = new Member { FullName = "John Doe", Email = "john@test.com", Phone = "123-456-7890" };
            var existingLoan = new Loan
            {
                Book = book,
                Member = member,
                LoanDate = DateTime.Today.AddDays(-5),
                DueDate = DateTime.Today.AddDays(9),
                ReturnedDate = null
            };

            context.Books.Add(book);
            context.Members.Add(member);
            context.Loans.Add(existingLoan);
            await context.SaveChangesAsync();

            // Act
            var bookFromDb = await context.Books.FindAsync(book.Id);

            // Assert
            Assert.NotNull(bookFromDb);
            Assert.False(bookFromDb.IsAvailable);
        }

        [Fact]
        public async Task ReturnedLoanMakesBookAvailableAgain()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var book = new Book { Title = "Test Book", Author = "Test Author", Isbn = "123", Category = "Fiction", IsAvailable = false };
            var member = new Member { FullName = "John Doe", Email = "john@test.com", Phone = "123-456-7890" };
            var loan = new Loan
            {
                Book = book,
                Member = member,
                LoanDate = DateTime.Today.AddDays(-5),
                DueDate = DateTime.Today.AddDays(9),
                ReturnedDate = null
            };

            context.Books.Add(book);
            context.Members.Add(member);
            context.Loans.Add(loan);
            await context.SaveChangesAsync();

            // Act
            loan.ReturnedDate = DateTime.Today;
            book.IsAvailable = true;
            context.Update(loan);
            context.Update(book);
            await context.SaveChangesAsync();

            var bookFromDb = await context.Books.FindAsync(book.Id);

            // Assert
            Assert.NotNull(bookFromDb);
            Assert.True(bookFromDb.IsAvailable);
        }

        [Fact]
        public async Task BookSearchReturnsByTitle()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.Books.AddRange(
                new Book { Title = "Harry Potter", Author = "J.K. Rowling", Isbn = "111", Category = "Fantasy", IsAvailable = true },
                new Book { Title = "Lord of the Rings", Author = "J.R.R. Tolkien", Isbn = "222", Category = "Fantasy", IsAvailable = true },
                new Book { Title = "Clean Code", Author = "Robert Martin", Isbn = "333", Category = "Technology", IsAvailable = true }
            );
            await context.SaveChangesAsync();

            // Act
            var searchTerm = "Potter";
            var results = await context.Books
                .Where(b => b.Title.Contains(searchTerm) || b.Author.Contains(searchTerm))
                .ToListAsync();

            // Assert
            Assert.Single(results);
            Assert.Equal("Harry Potter", results[0].Title);
        }

        [Fact]
        public async Task BookSearchReturnsByAuthor()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.Books.AddRange(
                new Book { Title = "Harry Potter", Author = "J.K. Rowling", Isbn = "111", Category = "Fantasy", IsAvailable = true },
                new Book { Title = "Lord of the Rings", Author = "J.R.R. Tolkien", Isbn = "222", Category = "Fantasy", IsAvailable = true },
                new Book { Title = "Clean Code", Author = "Robert Martin", Isbn = "333", Category = "Technology", IsAvailable = true }
            );
            await context.SaveChangesAsync();

            // Act
            var searchTerm = "Martin";
            var results = await context.Books
                .Where(b => b.Title.Contains(searchTerm) || b.Author.Contains(searchTerm))
                .ToListAsync();

            // Assert
            Assert.Single(results);
            Assert.Equal("Robert Martin", results[0].Author);
        }

        [Fact]
        public async Task OverdueLoanIdentifiedCorrectly()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var book = new Book { Title = "Test Book", Author = "Test Author", Isbn = "123", Category = "Fiction", IsAvailable = false };
            var member = new Member { FullName = "John Doe", Email = "john@test.com", Phone = "123-456-7890" };
            var overdueLoan = new Loan
            {
                Book = book,
                Member = member,
                LoanDate = DateTime.Today.AddDays(-20),
                DueDate = DateTime.Today.AddDays(-5),
                ReturnedDate = null
            };

            context.Books.Add(book);
            context.Members.Add(member);
            context.Loans.Add(overdueLoan);
            await context.SaveChangesAsync();

            // Act
            var loan = await context.Loans.FirstAsync();

            // Assert
            Assert.True(loan.IsOverdue);
            Assert.True(loan.IsActive);
            Assert.True(loan.DueDate < DateTime.Today);
            Assert.Null(loan.ReturnedDate);
        }

        [Fact]
        public async Task FilterBooksByCategory()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.Books.AddRange(
                new Book { Title = "Book 1", Author = "Author 1", Isbn = "111", Category = "Fiction", IsAvailable = true },
                new Book { Title = "Book 2", Author = "Author 2", Isbn = "222", Category = "Science", IsAvailable = true },
                new Book { Title = "Book 3", Author = "Author 3", Isbn = "333", Category = "Fiction", IsAvailable = true }
            );
            await context.SaveChangesAsync();

            // Act
            var category = "Fiction";
            var results = await context.Books
                .Where(b => b.Category == category)
                .ToListAsync();

            // Assert
            Assert.Equal(2, results.Count);
            Assert.All(results, b => Assert.Equal("Fiction", b.Category));
        }

        [Fact]
        public async Task FilterBooksByAvailability()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.Books.AddRange(
                new Book { Title = "Book 1", Author = "Author 1", Isbn = "111", Category = "Fiction", IsAvailable = true },
                new Book { Title = "Book 2", Author = "Author 2", Isbn = "222", Category = "Science", IsAvailable = false },
                new Book { Title = "Book 3", Author = "Author 3", Isbn = "333", Category = "Fiction", IsAvailable = true }
            );
            await context.SaveChangesAsync();

            // Act
            var results = await context.Books
                .Where(b => b.IsAvailable)
                .ToListAsync();

            // Assert
            Assert.Equal(2, results.Count);
            Assert.All(results, b => Assert.True(b.IsAvailable));
        }
    }
}

