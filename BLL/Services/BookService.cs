    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using BLL.DTO;
    using BLL.ServicesInterfaces;
    using DAL.Interfaces;
    using DAL.Models;

    namespace BLL.Services
    {
        public class BookService : IBookService
        {
            private readonly IBookRepository _bookRepository;
            private readonly IMapper _mapper;

            public BookService(IBookRepository bookRepository, IMapper mapper, IRepository<Author> authorRepository)
            {
                _bookRepository = bookRepository;
                _mapper = mapper;
     
            }

            public async Task<BookDTO> GetByIdAsync(int id)
            {
                var book = await _bookRepository.GetByIdAsync(id);
                return _mapper.Map<BookDTO>(book);
            }
            public async Task<BookDTO> GetByISBNAsync(string isbn)
            {
                var book = await _bookRepository.GetByISBNAsync(isbn);
                return _mapper.Map<BookDTO>(book);
            }
            public async Task<IEnumerable<BookDTO>> GetAllAsync()
            {
                var books = await _bookRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<BookDTO>>(books);
            }

            public async Task AddAsync(BookDTO bookDto)
            {
                var book = _mapper.Map<Book>(bookDto);
                await _bookRepository.AddAsync(book);
            }

            public async Task UpdateAsync(BookDTO bookDto)
            {
                var book = _mapper.Map<Book>(bookDto);
                await _bookRepository.UpdateAsync(book);
            }

            public async Task DeleteAsync(BookDTO bookDto)
            {
                var book = _mapper.Map<Book>(bookDto);
                await _bookRepository.DeleteAsync(book);
            }
        public async Task BorrowBookAsync(int bookId, int borrowerId)
        {
            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null)
            {
                throw new ArgumentException("Book not found");
            }

            if (book.BorrowerId != null)
            {
                throw new InvalidOperationException("Book is already borrowed.");
            }

            book.BorrowerId = borrowerId;
            book.BorrowedTime = DateTime.Now;
            book.ReturnTime = DateTime.Now.AddDays(7); // Default borrow period

            await _bookRepository.UpdateAsync(book);
        }

        public async Task AddBookImageAsync(int bookId, byte[] imageData)
            {
                var book = await _bookRepository.GetByIdAsync(bookId);
                if (book != null)
                {
                    book.ImageData = imageData;
                    await _bookRepository.UpdateAsync(book);
                }
            }
            public async Task<IEnumerable<BookDTO>> GetBooksByAuthorAsync(int authorId)
            {
                var books = await _bookRepository.GetBooksByAuthorAsync(authorId);
                return _mapper.Map<IEnumerable<BookDTO>>(books);
            }
        }
    }