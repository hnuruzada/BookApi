using AutoMapper;
using BookAPI.Apps.AdminApi.DTOs.AuthorDtos;
using BookAPI.Apps.AdminApi.DTOs.BookDtos;
using BookAPI.Apps.AdminApi.DTOs.GenreDtos;
using BookAPI.Data.Entity;

namespace BookAPI.Apps.AdminApi.Profiles
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Genre, GenreGetDto>();
            CreateMap<Genre, GenreInBookGetDto>();
            CreateMap<Author, AuthorGetDto>();
            CreateMap<Author,AuthorInBookGetDto>();
           
            CreateMap<Book, BookGetDto>();

            CreateMap<Book, BookPostDto>();
            CreateMap<Book,AuthorInBookGetDto>();
            CreateMap<Book, GenreInBookGetDto>();
            

        }
    }
}
