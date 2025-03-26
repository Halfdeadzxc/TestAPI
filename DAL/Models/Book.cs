using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class Book
    {
        public int Id { get; set; }

        
        public string ISBN { get; set; }

        
        public string Title { get; set; }

    
        public string Genre { get; set; }

       
        public string Description { get; set; }

        
        public int AuthorId { get; set; }
        public Author Author { get; set; }

      
        public DateTime BorrowedTime { get; set; }

       
        public DateTime ReturnTime { get; set; }

        public byte[] ImageData { get; set; }

        
        public int BorrowerId { get; set; } 
        public User Borrower { get; set; } 
    }
}
