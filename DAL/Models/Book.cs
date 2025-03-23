using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "ISBN must be 13 letters long")]
        public string ISBN { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int AuthorId { get; set; }
        public Author Author { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime BorrowedTime { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ReturnTime { get; set; }

        public byte[] ImageData { get; set; }

        [Required]
        public int BorrowerId { get; set; } 
        public User Borrower { get; set; } 
    }
}
