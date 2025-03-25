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
        public async Task<IActionResult> GetAll()
        {
            var authors = await _authorService.GetAllAsync();
            return Ok(authors);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var author = await _authorService.GetByIdAsync(id);
            return Ok(author);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] AuthorDTO authorDto)
        {
            await _authorService.AddAsync(authorDto);
            return CreatedAtAction(nameof(GetById), new { id = authorDto.Id }, authorDto);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminPolicy")] 
        public async Task<IActionResult> Update([FromBody] AuthorDTO authorDto)
        {
            await _authorService.UpdateAsync(authorDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromBody] AuthorDTO authorDto)
        {
            await _authorService.DeleteAsync(authorDto);
            return NoContent();
        }
    }
}
