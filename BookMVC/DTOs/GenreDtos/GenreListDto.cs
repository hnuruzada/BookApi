using System;
using System.Collections.Generic;

namespace BookMVC.DTOs.GenreDtos
{
    public class GenreListDto
    {
        public List<GenreListItemDto> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
