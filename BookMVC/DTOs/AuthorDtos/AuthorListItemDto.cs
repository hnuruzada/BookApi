using Microsoft.AspNetCore.Http;

namespace BookMVC.DTOs.AuthorDtos
{
    public class AuthorListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Image { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
