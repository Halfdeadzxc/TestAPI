using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<Book> GetByISBNAsync(string isbn);
        Task<IEnumerable<Book>> GetBooksByAuthorAsync(int authorId);
    }
}
