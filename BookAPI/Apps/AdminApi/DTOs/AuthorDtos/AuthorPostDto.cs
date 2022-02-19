using BookAPI.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace BookAPI.Apps.AdminApi.DTOs.AuthorDtos
{
    public class AuthorPostDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public IFormFile ImageFile { get; set; }
    }
    public class AuthorPostDtoValidator : AbstractValidator<AuthorPostDto>
    {
        public AuthorPostDtoValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(30).WithMessage("Name uzunlugu 30-den boyuk ola bilmez!")
                .NotEmpty().WithMessage("Name mecburidir!");
            RuleFor(x => x.Surname)
                .MaximumLength(30).WithMessage("Name uzunlugu 30-den boyuk ola bilmez!")
                .NotEmpty().WithMessage("Name mecburidir!");
            RuleFor(x => x.ImageFile).Custom((x, content) =>
            {
                
                    if (!x.IsImage())
                    {
                        content.AddFailure("ImageFile", "Choose correct image file");
                    }
                    if (!x.IsSizeOkay(2))
                    {
                        content.AddFailure("ImageFile", "File size must be max 2MB");
                    }
                
            }).NotEmpty().WithMessage("File is null choose image file");
        }
    }
}
