using FluentValidation;
using PictureLibrary.Model;

namespace PictureLibrary.API.Validation
{
    public class UserLoginValidator : AbstractValidator<UserLogin>
    {
        public UserLoginValidator()
        {
            _ = RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage("Username must not be empty");

            _ = RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password must not be empty");
        }
    }
}
