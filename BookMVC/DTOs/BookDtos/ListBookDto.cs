using BookMVC.DTOs.AuthorDtos;
using BookMVC.DTOs.GenreDtos;
using System.Collections.Generic;

namespace BookMVC.DTOs.BookDtos
{
    public class ListBookDto
    {
        public ListDto<AuthorListItemDto> Authors { get; set; }
        public ListDto<GenreListItemDto> Genres { get; set; }
        public BookPostDto postDtos { get; set; }
    }
}
