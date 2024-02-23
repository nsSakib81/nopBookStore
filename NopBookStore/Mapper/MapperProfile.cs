using AutoMapper;
using NopBookStore.Models;
using NopBookStore.ViewModels;

namespace NopBookStore.Mapper
{
    public class MapperProfile: Profile
    {
        public MapperProfile()
        {
            //CreateMap<Book, BookIndexPageViewModel>();
            //CreateMap<Book, BookEditViewModel>();
            CreateMap<BookEditViewModel, Book>()
            //.ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.BookId))
            //.ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            //.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            //.ForMember(dest => dest.publicationDate, opt => opt.MapFrom(src => src.publicationDate))
            //.ForMember(dest => dest.ISBN, opt => opt.MapFrom(src => src.ISBN))
            //.ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre))
            //.ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.Language));
            .ForMember(dest => dest.CoverPhoto, act => act.Ignore());
            //.ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
            //.ForMember(dest => dest.StockAmount, opt => opt.MapFrom(src => src.StockAmount));
        }
        
    }
}
