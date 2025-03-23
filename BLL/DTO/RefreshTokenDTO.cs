using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class RefreshTokenDTO
    {
        public string Token { get; set; }
        
        public int UserId { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
