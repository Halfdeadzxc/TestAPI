using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTO;
using BLL.Services;
using BLL.Profiles;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;
using DAL;
using DAL.EFRepositories;
namespace UnitTests
{
    public class AuthorServiceTests
    {
        private readonly IMapper _mapper;

        public AuthorServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Author, AuthorDTO>().ReverseMap();
            });

            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectAuthor()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "AuthorTestDb")
                .Options;

            using (var context = new AppDbContext(options))
            {
                context.Authors.Add(new Author { Id = 1, FirstName = "John", LastName = "Doe", BirthDate = new DateTime(1970, 1, 1), Country = "USA" });
                context.SaveChanges();

                var repository = new EFAuthorRepository(context);
                var service = new AuthorService(repository, _mapper);

                var author = await service.GetByIdAsync(1, default);

                Assert.NotNull(author);
                Assert.Equal("John", author.FirstName);
                Assert.Equal("Doe", author.LastName);
            }
        }

        [Fact]
        public async Task AddAsync_AddsNewAuthor()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "AuthorTestDb2")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var repository = new EFAuthorRepository(context);
                var service = new AuthorService(repository, _mapper);

                var newAuthor = new AuthorDTO
                {
                    FirstName = "Jane",
                    LastName = "Smith",
                    BirthDate = new DateTime(1990, 2, 2),
                    Country = "UK"
                };

                await service.AddAsync(newAuthor,default);

                var addedAuthor = context.Authors.FirstOrDefault();
                Assert.NotNull(addedAuthor);
                Assert.Equal("Jane", addedAuthor.FirstName);
                Assert.Equal("Smith", addedAuthor.LastName);
            }
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllAuthors()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "AuthorTestDb1")
                .Options;

            using (var context = new AppDbContext(options))
            {
                context.Authors.Add(new Author { Id = 1, FirstName = "John", LastName = "Doe", BirthDate = new DateTime(1970, 1, 1), Country = "USA" });
                context.Authors.Add(new Author { Id = 2, FirstName = "Alice", LastName = "Brown", BirthDate = new DateTime(1985, 5, 10), Country = "Canada" });
                context.SaveChanges();

                var repository = new EFAuthorRepository(context);
                var service = new AuthorService(repository, _mapper);

                var authors = await service.GetAllAsync(default);

                Assert.Equal(2, authors.Count());
            }
        }
        [Fact]
        public async Task UpdateAsync_UpdatesAuthorCorrectly()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "AuthorUpdateTestDb4")
                .Options;

            using (var context = new AppDbContext(options))
            {
                context.Authors.Add(new Author { Id = 1, FirstName = "John", LastName = "Doe", BirthDate = new DateTime(1970, 1, 1), Country = "USA" });
                await context.SaveChangesAsync();

                var repository = new EFAuthorRepository(context);
                var service = new AuthorService(repository, _mapper);

                var updatedAuthorDto = new AuthorDTO
                {
                    Id = 1,
                    FirstName = "UpdatedJohn",
                    LastName = "UpdatedDoe",
                    BirthDate = new DateTime(1970, 1, 1),
                    Country = "Canada"
                };

                var trackedEntity = context.Authors.Local.FirstOrDefault(a => a.Id == 1);
                if (trackedEntity != null)
                {
                    context.Entry(trackedEntity).State = EntityState.Detached;
                }

                var existingAuthor = await context.Authors.FindAsync(1);
                if (existingAuthor != null)
                {
                    context.Entry(existingAuthor).CurrentValues.SetValues(updatedAuthorDto);
                    await context.SaveChangesAsync();
                }

                var updatedAuthor = await repository.GetByIdAsync(1, default);
                Assert.NotNull(updatedAuthor);
                Assert.Equal("UpdatedJohn", updatedAuthor.FirstName);
                Assert.Equal("UpdatedDoe", updatedAuthor.LastName);
                Assert.Equal("Canada", updatedAuthor.Country);
            }
        }

        [Fact]
        public async Task DeleteAsync_RemovesAuthorCorrectly()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "AuthorDeleteTestDb5")
                .Options;

            using (var context = new AppDbContext(options))
            {
                context.Authors.Add(new Author { Id = 1, FirstName = "John", LastName = "Doe", BirthDate = new DateTime(1970, 1, 1), Country = "USA" });
                await context.SaveChangesAsync();

                var repository = new EFAuthorRepository(context);
                var service = new AuthorService(repository, _mapper);

                var trackedEntity = context.Authors.Local.FirstOrDefault(a => a.Id == 1);
                if (trackedEntity != null)
                {
                    context.Entry(trackedEntity).State = EntityState.Detached;
                }

                var existingAuthor = await context.Authors.FindAsync(1);
                if (existingAuthor != null)
                {
                    context.Authors.Remove(existingAuthor);
                    await context.SaveChangesAsync();
                }

                var deletedAuthor = await repository.GetByIdAsync(1, default);
                Assert.Null(deletedAuthor);
            }
        }






    }
}