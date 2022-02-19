using FluentValidation;

namespace BookAPI.Apps.AdminApi.DTOs.GenreDtos
{
    public class GenrePostDto
    {
        public string Name { get; set; }
    }
    public class GenrePostDtoValidator : AbstractValidator<GenrePostDto>
    {
        public GenrePostDtoValidator()
        {
            RuleFor(x => x.Name)
                  .MaximumLength(20).WithMessage("Name uzunlugu 20-den boyuk ola bilmez!")
                  .NotEmpty().WithMessage("Name mecburidir!");
        }
    }
}
