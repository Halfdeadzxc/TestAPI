using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Models;
namespace BLL.Profiles
{
    public class BookProfile:Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookDTO>()
                .ForMember(dest => dest.ISBN, opt => opt.MapFrom(src => src.ISBN))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
                .ForMember(dest => dest.BorrowedTime, opt => opt.MapFrom(src => src.BorrowedTime.ToString("yyyy-MM-ddTHH:mm:ss")))
                .ForMember(dest => dest.ReturnTime, opt => opt.MapFrom(src => src.ReturnTime.ToString("yyyy-MM-ddTHH:mm:ss")));

            CreateMap<BookDTO, Book>()
               .ForMember(dest => dest.ISBN, opt => opt.MapFrom(src => src.ISBN))
               .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
               .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre))
               .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
               .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
               .ForMember(dest => dest.BorrowedTime, opt => opt.MapFrom(src => src.BorrowedTime))
               .ForMember(dest => dest.ReturnTime, opt => opt.MapFrom(src => src.ReturnTime));
        }
    }
}
