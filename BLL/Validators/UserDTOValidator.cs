using BLL.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Validators
{
    public class UserDTOValidator:IValidator<UserDTO>
    {
        public  List<ValidationResult> Validate(UserDTO user)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(user);

            Validator.TryValidateObject(user, validationContext, validationResults, true);

            if (string.IsNullOrWhiteSpace(user.Role))
            {
                validationResults.Add(new ValidationResult(
                    "Role cannot be null, empty, or consist only of white-space characters.",
                    new[] { nameof(user.Role) }
                ));
            }

            return validationResults;
        }
    }
}
