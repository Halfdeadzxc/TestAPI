using BLL.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Validators
{
    public class RefreshTokenDTOValidator:IValidator<RefreshTokenDTO>
    {
        public List<ValidationResult> Validate(RefreshTokenDTO refreshToken)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(refreshToken);

            Validator.TryValidateObject(refreshToken, validationContext, validationResults, true);

            if (string.IsNullOrWhiteSpace(refreshToken.Token))
            {
                validationResults.Add(new ValidationResult(
                    "Token cannot be null, empty, or whitespace.",
                    new[] { nameof(refreshToken.Token) }
                ));
            }

            if (refreshToken.ExpiryDate < DateTime.Now)
            {
                validationResults.Add(new ValidationResult(
                    "ExpiryDate cannot be in the past.",
                    new[] { nameof(refreshToken.ExpiryDate) }
                ));
            }

            return validationResults;
        }
    }
}
