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
            private readonly IUserRepository _userRepository;
            public BookService(IBookRepository bookRepository, IMapper mapper, IRepository<Author> authorRepository, IUserRepository userRepository)
            {
            _userRepository = userRepository;
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
                var existingBook = await _bookRepository.GetByISBNAsync(bookDto.ISBN);
                if (existingBook != null)
                {
                    throw new ArgumentException("A book with the same ISBN already exists.");
                }
                var book = _mapper.Map<Book>(bookDto);
                await _bookRepository.AddAsync(book);
            }

            public async Task UpdateAsync(BookDTO bookDto)
            {
                var existingBook = await _bookRepository.GetByISBNAsync(bookDto.ISBN);
                if (existingBook != null)
                {
                    throw new ArgumentException("A book with the same ISBN already exists.");
                }
                var book = _mapper.Map<Book>(bookDto);
                await _bookRepository.UpdateAsync(book);
            }

            public async Task DeleteAsync(BookDTO bookDto)
            {
                var findbook = await _bookRepository.GetByIdAsync(bookDto.Id);
                if (findbook == null)
                {
                    throw new ArgumentException("Book not found");
                }
                var book = _mapper.Map<Book>(bookDto);
                await _bookRepository.DeleteAsync(book);
            }
        public async Task BorrowBookAsync(int bookId, int borrowerId, CancellationToken cancellationToken)
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
            var user = await _userRepository.GetUserByIdAsync(borrowerId, cancellationToken);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }
            book.BorrowerId = borrowerId;
            book.BorrowedTime = DateTime.Now;
            book.ReturnTime = DateTime.Now.AddDays(7); 

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