    using DAL.Interfaces;
    using DAL.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    namespace DAL.EFRepositories
    {
        public class EFBookRepository : IBookRepository
        {
            public AppDbContext Database { get; set; }
            public EFBookRepository(AppDbContext db) { Database = db; }

            public async Task<Book> GetByIdAsync(int Id, CancellationToken cancellationToken)
            {
                return await Database.Set<Book>().FindAsync(Id);
            }

            public async Task<IEnumerable<Book>> GetAllAsync(CancellationToken cancellationToken)
            {
                return await Database.Set<Book>().ToListAsync();
            }

            public async Task AddAsync(Book book, CancellationToken cancellationToken)
            {
                await Database.Set<Book>().AddAsync(book);
                await Database.SaveChangesAsync();
            }
            public async Task<Book> GetByISBNAsync(string isbn, CancellationToken cancellationToken)
            {
                return await Database.Set<Book>().FirstOrDefaultAsync(b => b.ISBN == isbn);
            }

            public async Task UpdateAsync(Book book, CancellationToken cancellationToken)
            {
               
                Database.Set<Book>().Update(book);
                await Database.SaveChangesAsync();
            }

            public async Task DeleteAsync(Book book, CancellationToken cancellationToken)
            {
               
                    Database.Set<Book>().Remove(book);
                    await Database.SaveChangesAsync();
                
            }
        public async Task<IEnumerable<Book>> GetBooksByAuthorAsync(int authorId, CancellationToken cancellationToken)
        {
            return await Database.Set<Book>()
                .Where(book => book.AuthorId == authorId)
                .ToListAsync();
        }

    }
    }
