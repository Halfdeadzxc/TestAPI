using AutoMapper;
using BLL.DTO;
using DAL.Models;

namespace BLL.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookDTO>()
                .ForMember(dest => dest.ISBN, opt => opt.MapFrom(src => src.ISBN))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author))
                .ForMember(dest => dest.BorrowedTime, opt => opt.MapFrom(src => src.BorrowedTime))
                .ForMember(dest => dest.ReturnTime, opt => opt.MapFrom(src => src.ReturnTime))
                .ForMember(dest => dest.BorrowerId, opt => opt.MapFrom(src => src.BorrowerId))
                .ForMember(dest => dest.Borrower, opt => opt.MapFrom(src => src.Borrower));

            CreateMap<BookDTO, Book>()
                .ForMember(dest => dest.ISBN, opt => opt.MapFrom(src => src.ISBN))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author))
                .ForMember(dest => dest.BorrowedTime, opt => opt.MapFrom(src => src.BorrowedTime))
                .ForMember(dest => dest.ReturnTime, opt => opt.MapFrom(src => src.ReturnTime))
                .ForMember(dest => dest.BorrowerId, opt => opt.MapFrom(src => src.BorrowerId))
                .ForMember(dest => dest.Borrower, opt => opt.MapFrom(src => src.Borrower));
        }
    }
}
