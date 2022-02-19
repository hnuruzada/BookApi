using BookAPI.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace BookAPI.Apps.AdminApi.DTOs.BookDtos
{
    public class BookPostDto
    {
        public int AuthorId { get; set; }
        public int GenreId { get; set; }
        public string Name { get; set; }
        public IFormFile ImageFile { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CostPrice { get; set; }
        public int PageCount { get; set; }
        public string Language { get; set; }
    }
    public class BookPostDtoValidator : AbstractValidator<BookPostDto>
    {
        public BookPostDtoValidator()
        {
            RuleFor(x=>x.PageCount).NotEmpty();
            RuleFor(x => x.Language).NotEmpty().MinimumLength(5).MaximumLength(50).WithMessage("uzunluq min 5 max 50 olmalidir");
            RuleFor(x => x.AuthorId).NotEmpty().GreaterThanOrEqualTo(1);
            RuleFor(x => x.GenreId).NotEmpty().GreaterThanOrEqualTo(1);
            RuleFor(x => x.Name)
                .MaximumLength(50).WithMessage("Uzunluq max 50 ola biler!")
                .NotEmpty().WithMessage("Name mecburidir!");
            RuleFor(x => x.CostPrice)
                .GreaterThanOrEqualTo(0).WithMessage("CostPrice 0-dan asagi ola bilmez!")
                .NotEmpty().WithMessage("CostPrice mecburidir!");

            RuleFor(x => x.SalePrice)
                .GreaterThanOrEqualTo(0).WithMessage("SalePrice 0-dan asagi ola bilmez!")
                .NotEmpty().WithMessage("SalePrice mecburidir!");
            RuleFor(x => x).Custom((x, context) =>
            {
                if (x.CostPrice > x.SalePrice)
                    context.AddFailure("CostPrice", "CostPrice SalePrice-dan boyuk ola bilmez");
            });
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
