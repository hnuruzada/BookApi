using FluentValidation;

namespace BookAPI.Apps.UserApi.DTOs.AccountDtos
{
    public class LoginDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().MinimumLength(4).MaximumLength(20);
            RuleFor(x=>x.Password).NotEmpty().MinimumLength(8).MaximumLength(25);
        }
    }
}
