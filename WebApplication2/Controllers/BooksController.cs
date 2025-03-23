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
        public async Task<IActionResult> GetAll()
        {
            var books = await _bookService.GetAllAsync();
            return Ok(books); 
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _bookService.GetByIdAsync(id);
            return Ok(book); 
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] BookDTO bookDto)
        {
            await _bookService.AddAsync(bookDto);
            return CreatedAtAction(nameof(GetById), new { id = bookDto.Id }, bookDto); 
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BookDTO bookDto)
        {
            await _bookService.UpdateAsync(bookDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromBody] BookDTO bookDto)
        {
            await _bookService.DeleteAsync(bookDto);
            return NoContent(); 
        }

        [HttpPost("Borrow/{bookId}/{borrowerId}")]
        public async Task<IActionResult> BorrowBook(int bookId, int borrowerId)
        {
            await _bookService.BorrowBookAsync(bookId, borrowerId);
            return NoContent(); 
        }
    }
}
