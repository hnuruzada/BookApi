using System.Collections.Generic;

namespace BookMVC.DTOs.BookDtos
{
    public class BookListDto
    {
        public List<BookListItemDto> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
