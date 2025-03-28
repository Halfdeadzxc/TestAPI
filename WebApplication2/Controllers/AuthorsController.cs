using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BLL.DTO;
using BLL.ServicesInterfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly IService<AuthorDTO> _authorService;

        public AuthorController(IService<AuthorDTO> authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var authors = await _authorService.GetAllAsync(cancellationToken);
            return Ok(authors);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var author = await _authorService.GetByIdAsync(id,cancellationToken);
            return Ok(author);
        }

        [HttpPost]
        [Authorize]

        public async Task<IActionResult> Add([FromBody] AuthorDTO authorDto, CancellationToken cancellationToken)
        {
            await _authorService.AddAsync(authorDto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = authorDto.Id }, authorDto);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminPolicy")]

        public async Task<IActionResult> Update([FromBody] AuthorDTO authorDto, CancellationToken cancellationToken)
        {
            await _authorService.UpdateAsync(authorDto, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromBody] AuthorDTO authorDto, CancellationToken cancellationToken)
        {
            await _authorService.DeleteAsync(authorDto, cancellationToken);
            return NoContent();
        }
      
    }
}
