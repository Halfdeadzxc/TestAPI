using BLL.DTO;
using DAL.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Validators
{
    public class AuthorDTOValidator:IValidator<AuthorDTO>
    {
        public  List<ValidationResult> Validate(AuthorDTO author)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(author);

            Validator.TryValidateObject(author, validationContext, validationResults, true);

            if (author.BirthDate > DateTime.Now)
            {
                validationResults.Add(new ValidationResult(
                    "BirthDate cannot be in the future.",
                    new[] { nameof(author.BirthDate) }
                ));
            }

            return validationResults;
        }
    }
}
