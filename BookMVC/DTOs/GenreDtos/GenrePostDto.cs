using System.ComponentModel.DataAnnotations;

namespace BookMVC.DTOs.GenreDtos
{
    public class GenrePostDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = ("This filed can not be empty"))]
        [StringLength(maximumLength: 20)]
        public string Name { get; set; }
    }
}
