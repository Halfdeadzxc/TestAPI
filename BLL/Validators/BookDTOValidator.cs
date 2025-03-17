using BLL.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Validators
{
    public class BookDTOValidator :IValidator<BookDTO>
    {
        public  List<ValidationResult> Validate(BookDTO book)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(book);

            Validator.TryValidateObject(book, validationContext, validationResults, true);

            if (book.BorrowedTime > book.ReturnTime)
            {   
                validationResults.Add(new ValidationResult(
                    "BorrowedTime must be earlier than ReturnTime.",
                    new[] { nameof(book.BorrowedTime), nameof(book.ReturnTime) }
                ));
            }

            return validationResults;
        }
    }
}
