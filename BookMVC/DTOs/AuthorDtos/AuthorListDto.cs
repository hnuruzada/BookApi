using System.Collections.Generic;

namespace BookMVC.DTOs.AuthorDtos
{
    public class AuthorListDto
    {
        public List<AuthorListItemDto> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
