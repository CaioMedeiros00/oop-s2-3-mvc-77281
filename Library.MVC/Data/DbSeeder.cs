using Library.Domain;
using Microsoft.AspNetCore.Identity;

namespace Library.MVC.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Seed Roles
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Seed Admin User
            var adminEmail = "admin@library.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(adminUser, "Admin@123");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // Seed Books
            if (!context.Books.Any())
            {
                var books = new List<Book>
                {
                    new Book { Id = 1, Title = "A Court of Thorns and Roses", Author = "Sarah J. Maas", Isbn = "9781547604173", Category = "Romantasy", IsAvailable = true },
                    new Book { Id = 2, Title = "Throne of Glass", Author = "Sarah J. Maas", Isbn = "9781619630345", Category = "Fantasy", IsAvailable = false },
                    new Book { Id = 3, Title = "House of Earth and Blood", Author = "Sarah J. Maas", Isbn = "9781635574043", Category = "Urban Fantasy", IsAvailable = true },
                    new Book { Id = 4, Title = "Fourth Wing", Author = "Rebecca Yarros", Isbn = "9781649374042", Category = "Romantasy", IsAvailable = true },
                    new Book { Id = 5, Title = "The Serpent and the Wings of Night", Author = "Carissa Broadbent", Isbn = "9781737472608", Category = "Dark Fantasy", IsAvailable = false },
                    new Book { Id = 6, Title = "The Cruel Prince", Author = "Holly Black", Isbn = "9780316310277", Category = "Fantasy", IsAvailable = true },
                    new Book { Id = 7, Title = "From Blood and Ash", Author = "Jennifer L. Armentrout", Isbn = "9781952457005", Category = "Romantasy", IsAvailable = false },
                    new Book { Id = 8, Title = "Iron Widow", Author = "Xiran Jay Zhao", Isbn = "9780735269934", Category = "Science Fantasy", IsAvailable = true },
                    new Book { Id = 9, Title = "Red Rising", Author = "Pierce Brown", Isbn = "9780345539786", Category = "Science Fiction", IsAvailable = true },
                    new Book { Id = 10, Title = "The Poppy War", Author = "R. F. Kuang", Isbn = "9780062662569", Category = "Dark Fantasy", IsAvailable = false },
                    new Book { Id = 11, Title = "Gideon the Ninth", Author = "Tamsyn Muir", Isbn = "9781250313195", Category = "Science Fantasy", IsAvailable = true },
                    new Book { Id = 12, Title = "An Ember in the Ashes", Author = "Sabaa Tahir", Isbn = "9781595148032", Category = "Fantasy", IsAvailable = true },
                    new Book { Id = 13, Title = "Mistborn: The Final Empire", Author = "Brandon Sanderson", Isbn = "9780765311788", Category = "Epic Fantasy", IsAvailable = true },
                    new Book { Id = 14, Title = "The Way of Kings", Author = "Brandon Sanderson", Isbn = "9780765326355", Category = "Epic Fantasy", IsAvailable = false },
                    new Book { Id = 15, Title = "Warbreaker", Author = "Brandon Sanderson", Isbn = "9780765320308", Category = "Fantasy", IsAvailable = true },
                    new Book { Id = 16, Title = "Skyward", Author = "Brandon Sanderson", Isbn = "9780399555770", Category = "Science Fiction", IsAvailable = true },
                    new Book { Id = 17, Title = "The Name of the Wind", Author = "Patrick Rothfuss", Isbn = "9780756404741", Category = "Epic Fantasy", IsAvailable = false },
                    new Book { Id = 18, Title = "The Lies of Locke Lamora", Author = "Scott Lynch", Isbn = "9780553588941", Category = "Fantasy", IsAvailable = true },
                    new Book { Id = 19, Title = "The Blade Itself", Author = "Joe Abercrombie", Isbn = "9780575079793", Category = "Grimdark Fantasy", IsAvailable = true },
                    new Book { Id = 20, Title = "The Priory of the Orange Tree", Author = "Samantha Shannon", Isbn = "9781635570298", Category = "Epic Fantasy", IsAvailable = false },
                    new Book { Id = 21, Title = "Nevernight", Author = "Jay Kristoff", Isbn = "9781250073020", Category = "Dark Fantasy", IsAvailable = true },
                    new Book { Id = 22, Title = "The Jasmine Throne", Author = "Tasha Suri", Isbn = "9780316366113", Category = "Epic Fantasy", IsAvailable = false },
                    new Book { Id = 23, Title = "The Last Wish", Author = "Andrzej Sapkowski", Isbn = "9780316029186", Category = "Fantasy", IsAvailable = true }
                };

                context.Books.AddRange(books);
                await context.SaveChangesAsync();
            }

            // Seed Members
            if (!context.Members.Any())
            {
                var members = new List<Member>
                {
                    new Member { Id = 1, FullName = "Caio Medeiros", Email = "caio.medeiros@example.com", Phone = "+353861234567" },
                    new Member { Id = 2, FullName = "Isabela Costa", Email = "isabela.costa@example.com", Phone = "+353861112233" },
                    new Member { Id = 3, FullName = "Lucas Ferreira", Email = "lucas.ferreira@example.com", Phone = "+353869998877" },
                    new Member { Id = 4, FullName = "Ana Ribeiro", Email = "ana.ribeiro@example.com", Phone = "+353861234890" },
                    new Member { Id = 5, FullName = "Mateus Oliveira", Email = "mateus.oliveira@example.com", Phone = "+353861223344" },
                    new Member { Id = 6, FullName = "Beatriz Martins", Email = "beatriz.martins@example.com", Phone = "+353861234111" },
                    new Member { Id = 7, FullName = "Rafael Santos", Email = "rafael.santos@example.com", Phone = "+353861234222" },
                    new Member { Id = 8, FullName = "Mariana Gomes", Email = "mariana.gomes@example.com", Phone = "+353861234333" },
                    new Member { Id = 9, FullName = "Pedro Almeida", Email = "pedro.almeida@example.com", Phone = "+353861234444" },
                    new Member { Id = 10, FullName = "Sofia Nunes", Email = "sofia.nunes@example.com", Phone = "+353861234555" },
                    new Member { Id = 11, FullName = "Gabriel Pinto", Email = "gabriel.pinto@example.com", Phone = "+353861234666" },
                    new Member { Id = 12, FullName = "Laura Carvalho", Email = "laura.carvalho@example.com", Phone = "+353861234777" }
                };

                context.Members.AddRange(members);
                await context.SaveChangesAsync();
            }

            // Seed Loans (Active Loans for Members)
            if (!context.Loans.Any())
            {
                var loans = new List<Loan>
                {
                    // Caio Medeiros loans
                    new Loan { MemberId = 1, BookId = 2, LoanDate = DateTime.Today.AddDays(-10), DueDate = DateTime.Today.AddDays(4) },
                    new Loan { MemberId = 1, BookId = 14, LoanDate = DateTime.Today.AddDays(-10), DueDate = DateTime.Today.AddDays(4) },
                    new Loan { MemberId = 1, BookId = 20, LoanDate = DateTime.Today.AddDays(-10), DueDate = DateTime.Today.AddDays(4) },

                    // Isabela Costa loans
                    new Loan { MemberId = 2, BookId = 5, LoanDate = DateTime.Today.AddDays(-8), DueDate = DateTime.Today.AddDays(6) },
                    new Loan { MemberId = 2, BookId = 10, LoanDate = DateTime.Today.AddDays(-8), DueDate = DateTime.Today.AddDays(6) },
                    new Loan { MemberId = 2, BookId = 22, LoanDate = DateTime.Today.AddDays(-8), DueDate = DateTime.Today.AddDays(6) },

                    // Lucas Ferreira loans
                    new Loan { MemberId = 3, BookId = 7, LoanDate = DateTime.Today.AddDays(-12), DueDate = DateTime.Today.AddDays(2) },
                    new Loan { MemberId = 3, BookId = 17, LoanDate = DateTime.Today.AddDays(-12), DueDate = DateTime.Today.AddDays(2) },

                    // Ana Ribeiro loans
                    new Loan { MemberId = 4, BookId = 19, LoanDate = DateTime.Today.AddDays(-5), DueDate = DateTime.Today.AddDays(9) },

                    // Mateus Oliveira loans
                    new Loan { MemberId = 5, BookId = 2, LoanDate = DateTime.Today.AddDays(-7), DueDate = DateTime.Today.AddDays(7) },
                    new Loan { MemberId = 5, BookId = 20, LoanDate = DateTime.Today.AddDays(-7), DueDate = DateTime.Today.AddDays(7) },

                    // Beatriz Martins loans
                    new Loan { MemberId = 6, BookId = 10, LoanDate = DateTime.Today.AddDays(-9), DueDate = DateTime.Today.AddDays(5) },
                    new Loan { MemberId = 6, BookId = 22, LoanDate = DateTime.Today.AddDays(-9), DueDate = DateTime.Today.AddDays(5) },

                    // Rafael Santos loans
                    new Loan { MemberId = 7, BookId = 7, LoanDate = DateTime.Today.AddDays(-6), DueDate = DateTime.Today.AddDays(8) },
                    new Loan { MemberId = 7, BookId = 19, LoanDate = DateTime.Today.AddDays(-6), DueDate = DateTime.Today.AddDays(8) },

                    // Mariana Gomes loans
                    new Loan { MemberId = 8, BookId = 17, LoanDate = DateTime.Today.AddDays(-11), DueDate = DateTime.Today.AddDays(3) },

                    // Pedro Almeida loans
                    new Loan { MemberId = 9, BookId = 20, LoanDate = DateTime.Today.AddDays(-4), DueDate = DateTime.Today.AddDays(10) },
                    new Loan { MemberId = 9, BookId = 14, LoanDate = DateTime.Today.AddDays(-4), DueDate = DateTime.Today.AddDays(10) },

                    // Sofia Nunes loans
                    new Loan { MemberId = 10, BookId = 5, LoanDate = DateTime.Today.AddDays(-13), DueDate = DateTime.Today.AddDays(1) },

                    // Gabriel Pinto loans
                    new Loan { MemberId = 11, BookId = 19, LoanDate = DateTime.Today.AddDays(-3), DueDate = DateTime.Today.AddDays(11) },
                    new Loan { MemberId = 11, BookId = 22, LoanDate = DateTime.Today.AddDays(-3), DueDate = DateTime.Today.AddDays(11) },

                    // Laura Carvalho loans
                    new Loan { MemberId = 12, BookId = 2, LoanDate = DateTime.Today.AddDays(-14), DueDate = DateTime.Today },
                    new Loan { MemberId = 12, BookId = 10, LoanDate = DateTime.Today.AddDays(-14), DueDate = DateTime.Today }
                };

                context.Loans.AddRange(loans);
                await context.SaveChangesAsync();
            }
        }
    }
}
