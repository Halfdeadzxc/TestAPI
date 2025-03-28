using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using DAL.EFRepositories;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;
using DAL;

namespace UnitTests
{
    public class EFAuthorRepositoryTests
    {
        private DbContextOptions<AppDbContext> CreateNewContextOptions(string databaseName)
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: databaseName)
                .Options;
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectAuthor()
        {
            var options = CreateNewContextOptions("GetByIdTestDb");

            using (var context = new AppDbContext(options))
            {
                
                var author = new Author { Id = 1, FirstName = "John", LastName = "Doe", BirthDate = new DateTime(1970, 1, 1), Country = "USA" };
                context.Authors.Add(author);
                context.SaveChanges();

                var repository = new EFAuthorRepository(context);

                var result = await repository.GetByIdAsync(1, default);

                Assert.NotNull(result);
                Assert.Equal("John", result.FirstName);
                Assert.Equal("Doe", result.LastName);
            }
        }

        [Fact]
        public async Task AddAsync_AddsAuthorToDatabase()
        {
            var options = CreateNewContextOptions("AddTestDb2");

            using (var context = new AppDbContext(options))
            {
                var repository = new EFAuthorRepository(context);
                var newAuthor = new Author
                {
                    FirstName = "Jane",
                    LastName = "Smith",
                    BirthDate = new DateTime(1990, 2, 2),
                    Country = "UK"
                };

                await repository.AddAsync(newAuthor, default);

                var authorInDb = context.Authors.FirstOrDefault(a => a.FirstName == "Jane" && a.LastName == "Smith");
                Assert.NotNull(authorInDb);
                Assert.Equal("Jane", authorInDb.FirstName);
                Assert.Equal("Smith", authorInDb.LastName);
                Assert.Equal("UK", authorInDb.Country);
            }
        }
    }
}
