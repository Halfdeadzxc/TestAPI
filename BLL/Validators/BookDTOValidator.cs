using BLL.DTO;
using FluentValidation;
using System;

namespace BLL.Validators
{
    public class BookDTOValidator : AbstractValidator<BookDTO>
    {
        public BookDTOValidator()
        {
            RuleFor(book => book.ISBN)
                .NotEmpty().WithMessage("ISBN cannot be empty.")
                .Length(10, 13).WithMessage("ISBN must be either 10 or 13 characters long.")
                .Matches(@"^\d{10}(\d{3})?$").WithMessage("ISBN must contain only digits.");

            RuleFor(book => book.Title)
                .NotEmpty().WithMessage("Title cannot be empty.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

            RuleFor(book => book.Genre)
                .NotEmpty().WithMessage("Genre cannot be empty.")
                .MaximumLength(50).WithMessage("Genre cannot exceed 50 characters.");

            RuleFor(book => book.Description)
                .NotEmpty().WithMessage("Description cannot be empty.")
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(book => book.AuthorId)
                .GreaterThan(0).WithMessage("AuthorId must be greater than 0.");

            RuleFor(book => book)
                .Must(book => book.BorrowedTime <= book.ReturnTime)
                .WithMessage("BorrowedTime must be earlier than or equal to ReturnTime.");

            RuleFor(book => book.BorrowedTime)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("BorrowedTime cannot be in the future.");
        }
    }
}
