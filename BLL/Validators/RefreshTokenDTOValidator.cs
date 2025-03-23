using BLL.DTO;
using FluentValidation;
using System;

namespace BLL.Validators
{
    public class RefreshTokenDTOValidator : AbstractValidator<RefreshTokenDTO>
    {
        public RefreshTokenDTOValidator()
        {
            RuleFor(refreshToken => refreshToken.Token)
                .NotEmpty().WithMessage("Token cannot be null, empty, or whitespace.")
                .NotNull().WithMessage("Token cannot be null.");

            RuleFor(refreshToken => refreshToken.ExpiryDate)
                .GreaterThan(DateTime.Now).WithMessage("ExpiryDate cannot be in the past.");
        }
    }
}
