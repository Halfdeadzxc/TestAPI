using BLL.DTO;
using FluentValidation;
using System;

namespace BLL.Validators
{
    public class AuthorDTOValidator : AbstractValidator<AuthorDTO>
    {
        public AuthorDTOValidator()
        {
            RuleFor(author => author.FirstName)
                .NotEmpty().WithMessage("First Name cannot be empty.")
                .MaximumLength(50).WithMessage("First Name cannot exceed 50 characters.");

            RuleFor(author => author.LastName)
                .NotEmpty().WithMessage("Last Name cannot be empty.")
                .MaximumLength(50).WithMessage("Last Name cannot exceed 50 characters.");

            RuleFor(author => author.BirthDate)
                .LessThan(DateTime.Now).WithMessage("Birth Date cannot be in the future.")
                .GreaterThan(DateTime.Now.AddYears(-150)).WithMessage("Birth Date must be within the last 150 years.");

            RuleFor(author => author.Country)
                .NotEmpty().WithMessage("Country cannot be empty.")
                .MaximumLength(50).WithMessage("Country cannot exceed 50 characters.");
        }
    }
}
    