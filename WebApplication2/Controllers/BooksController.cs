using Microsoft.AspNetCore.Mvc;
using BLL.DTO;
using BLL.ServicesInterfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var books = await _bookService.GetAllAsync(cancellationToken);
            return Ok(books); 
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var book = await _bookService.GetByIdAsync(id, cancellationToken);
            return Ok(book); 
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] BookDTO bookDto, CancellationToken cancellationToken)
        {
            await _bookService.AddAsync(bookDto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = bookDto.Id }, bookDto); 
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BookDTO bookDto, CancellationToken cancellationToken)
        {
            await _bookService.UpdateAsync(bookDto, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromBody] BookDTO bookDto, CancellationToken cancellationToken)
        {
            await _bookService.DeleteAsync(bookDto, cancellationToken);
            return NoContent(); 
        }

        [HttpPost("Borrow/{bookId}/{borrowerId}")]
        public async Task<IActionResult> BorrowBook(int bookId, int borrowerId, CancellationToken cancellationToken)
        {
            await _bookService.BorrowBookAsync(bookId, borrowerId, cancellationToken);
            return NoContent(); 
        }
    }
}
