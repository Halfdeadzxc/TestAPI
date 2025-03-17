using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } 

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } 

        [Required]
        [DataType(DataType.Date)]
        [Range(typeof(DateTime), "1/1/1900", "12/31/2100", ErrorMessage = "date must be between 01/01/1900 and 12/31/2100")]
        public DateTime BirthDate { get; set; }

        [Required]
        public string Country { get; set; }
    }
}
