using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EFRepositories
{
    public class EFAuthorRepository : IRepository<Author>
    {
        public AppDbContext Database { get; set; }

        public EFAuthorRepository(AppDbContext Db)
        {
            Database = Db;
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await Database.Set<Author>().ToListAsync();
        }

        public async Task<Author> GetByIdAsync(int id)
        {
            return await Database.Set<Author>().FindAsync(id);
        }

        public async Task AddAsync(Author author)
        {
            await Database.Set<Author>().AddAsync(author);
            await Database.SaveChangesAsync();
        }

        public async Task UpdateAsync(Author author)
        {
            Database.Set<Author>().Update(author);
            await Database.SaveChangesAsync();
        }

        public async Task DeleteAsync(Author author)
        {
            Database.Set<Author>().Remove(author);
            await Database.SaveChangesAsync();
        }
    }
}
