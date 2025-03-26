using AutoMapper;
using BLL.DTO;
using BLL.ServicesInterfaces;
using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class AuthorService : IService<AuthorDTO>
    {
        private readonly IRepository<Author> _authorRepository;
        private readonly IMapper _mapper;

        public AuthorService(IRepository<Author> authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public async Task<AuthorDTO> GetByIdAsync(int id)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            return _mapper.Map<AuthorDTO>(author);
        }

        public async Task<IEnumerable<AuthorDTO>> GetAllAsync()
        {
            var authors = await _authorRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<AuthorDTO>>(authors);
        }

        public async Task AddAsync(AuthorDTO authorDto)
        {
            var author = _mapper.Map<Author>(authorDto);
            await _authorRepository.AddAsync(author);
        }

        public async Task UpdateAsync(AuthorDTO authorDto)
        {
            var existingAuthor = await _authorRepository.GetByIdAsync(authorDto.Id);
            if (existingAuthor == null)
            {
                throw new ArgumentException($"Author with Id {authorDto.Id} does not exist.");
            }
            var author = _mapper.Map<Author>(authorDto);
            await _authorRepository.UpdateAsync(author);
        }

        public async Task DeleteAsync(AuthorDTO authorDto)
        {
            var existingAuthor = await _authorRepository.GetByIdAsync(authorDto.Id);
            if (existingAuthor == null)
            {
                throw new ArgumentException($"Author with Id {authorDto.Id} does not exist.");
            }
            var author = _mapper.Map<Author>(authorDto);
            await _authorRepository.DeleteAsync(author);
        }
    }
}
