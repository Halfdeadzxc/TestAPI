using BLL.DTO;
using BLL.ServicesInterfaces;
using BLL.Validators;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IValidator<BookDTO> _bookValidator;

        public BooksController(IBookService bookService, IValidator<BookDTO> bookValidator)
        {
            _bookService = bookService;
            _bookValidator = bookValidator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetAll()
        {
            var books = await _bookService.GetAllAsync();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> GetById(int id)
        {
            var book = await _bookService.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpGet("isbn/{isbn}")]
        public async Task<ActionResult<BookDTO>> GetByISBN(string isbn)
        {
            var book = await _bookService.GetByISBNAsync(isbn);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] BookDTO bookDto)
        {
            if (bookDto == null)
            {
                return BadRequest("Invalid book data.");
            }

            var validationResults = _bookValidator.Validate(bookDto);
            if (validationResults.Count > 0)
            {
                return BadRequest(validationResults);
            }

            await _bookService.AddAsync(bookDto);
            return CreatedAtAction(nameof(GetById), new { id = bookDto.Id }, bookDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] BookDTO bookDto)
        {
            if (bookDto == null || bookDto.Id != id)
            {
                return BadRequest("Invalid book data.");
            }

            var existingBook = await _bookService.GetByIdAsync(id);
            if (existingBook == null)
            {
                return NotFound();
            }

            var validationResults = _bookValidator.Validate(bookDto);
            if (validationResults.Count > 0)
            {
                return BadRequest(validationResults);
            }

            await _bookService.UpdateAsync(bookDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var book = await _bookService.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            await _bookService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/borrow")]
        public async Task<ActionResult> BorrowBook(int id)
        {
            var book = await _bookService.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            await _bookService.BorrowBookAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/image")]
        public async Task<ActionResult> AddBookImage(int id, [FromBody] byte[] imageData)
        {
            var book = await _bookService.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            await _bookService.AddBookImageAsync(id, imageData);
            return NoContent();
        }

        [HttpGet("author/{authorId}")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooksByAuthor(int authorId)
        {
            var books = await _bookService.GetBooksByAuthorAsync(authorId);
            return Ok(books);
        }
    }
}
