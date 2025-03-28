using AutoMapper;
using BLL.DTO;
using BLL.ServicesInterfaces;
using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

        public async Task<AuthorDTO> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var author = await _authorRepository.GetByIdAsync(id, cancellationToken);
            return _mapper.Map<AuthorDTO>(author);
        }

        public async Task<IEnumerable<AuthorDTO>> GetAllAsync(CancellationToken cancellationToken)
        {
            var authors = await _authorRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<AuthorDTO>>(authors);
        }

        public async Task AddAsync(AuthorDTO authorDto, CancellationToken cancellationToken)
        {
            var author = _mapper.Map<Author>(authorDto);
            await _authorRepository.AddAsync(author, cancellationToken);
        }

        public async Task UpdateAsync(AuthorDTO authorDto, CancellationToken cancellationToken)
        {
            var existingAuthor = await _authorRepository.GetByIdAsync(authorDto.Id, cancellationToken);
            if (existingAuthor == null)
            {
                throw new ArgumentException($"Author with Id {authorDto.Id} does not exist.");
            }
            var author = _mapper.Map<Author>(authorDto);
            await _authorRepository.UpdateAsync(author, cancellationToken);
        }

        public async Task DeleteAsync(AuthorDTO authorDto, CancellationToken cancellationToken)
        {
            var existingAuthor = await _authorRepository.GetByIdAsync(authorDto.Id, cancellationToken);
            if (existingAuthor == null)
            {
                throw new ArgumentException($"Author with Id {authorDto.Id} does not exist.");
            }
            var author = _mapper.Map<Author>(authorDto);
            await _authorRepository.DeleteAsync(author, cancellationToken);
        }
    }
}
