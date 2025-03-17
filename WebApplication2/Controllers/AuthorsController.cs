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
    public class AuthorsController : ControllerBase
    {
        private readonly IService<AuthorDTO> _authorService;
        private readonly IValidator<AuthorDTO> _authorValidator;

        public AuthorsController(IService<AuthorDTO> authorService, IValidator<AuthorDTO> authorValidator)
        {
            _authorService = authorService;
            _authorValidator = authorValidator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDTO>>> GetAll()
        {
            var authors = await _authorService.GetAllAsync();
            return Ok(authors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDTO>> GetById(int id)
        {
            var author = await _authorService.GetByIdAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            return Ok(author);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] AuthorDTO authorDto)
        {
            if (authorDto == null)
            {
                return BadRequest("Invalid author data.");
            }

            var validationResults = _authorValidator.Validate(authorDto);
            if (validationResults.Count > 0)
            {
                return BadRequest(validationResults);
            }

            await _authorService.AddAsync(authorDto);
            return CreatedAtAction(nameof(GetById), new { id = authorDto.Id }, authorDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] AuthorDTO authorDto)
        {
            if (authorDto == null || authorDto.Id != id)
            {
                return BadRequest("Invalid author data.");
            }

            var existingAuthor = await _authorService.GetByIdAsync(id);
            if (existingAuthor == null)
            {
                return NotFound();
            }

            var validationResults = _authorValidator.Validate(authorDto);
            if (validationResults.Count > 0)
            {
                return BadRequest(validationResults);
            }

            await _authorService.UpdateAsync(authorDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var author = await _authorService.GetByIdAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            await _authorService.DeleteAsync(id);
            return NoContent();
        }
    }
}
