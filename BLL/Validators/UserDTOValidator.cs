using BLL.DTO;
using FluentValidation;

namespace BLL.Validators
{
    public class UserDTOValidator : AbstractValidator<UserDTO>
    {
        public UserDTOValidator()
        {
            RuleFor(user => user.Username)
                .NotEmpty().WithMessage("Username cannot be empty.")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
                .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.");

            RuleFor(user => user.PasswordHash)
                .NotEmpty().WithMessage("PasswordHash cannot be empty.")
                .MinimumLength(8).WithMessage("PasswordHash must be at least 8 characters long.");

            RuleFor(user => user.Role)
                .NotEmpty().WithMessage("Role cannot be empty.")
                .Must(role => role == "Admin" || role == "User")
                .WithMessage("Role must be either 'Admin' or 'User'.");
        }
    }
}
