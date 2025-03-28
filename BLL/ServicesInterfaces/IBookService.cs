using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ServicesInterfaces
{
    public interface IBookService:IService<BookDTO>
    {
        Task AddBookImageAsync(int bookId, byte[] imageData, CancellationToken cancellationToken);
        Task BorrowBookAsync(int bookId, int borrowerId, CancellationToken cancellationToken);
        Task<BookDTO> GetByISBNAsync(string isbn, CancellationToken cancellationToken);
        Task<IEnumerable<BookDTO>> GetBooksByAuthorAsync(int authorId, CancellationToken cancellationToken);

    }
}
