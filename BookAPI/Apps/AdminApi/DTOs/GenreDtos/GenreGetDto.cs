using System;

namespace BookAPI.Apps.AdminApi.DTOs.GenreDtos
{
    public class GenreGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public int BooksCount { get; set; }
    }
}
