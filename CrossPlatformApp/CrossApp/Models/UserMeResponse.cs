using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossApp.Models
{
    public class UserMeResponse
    {
        public int Id { get; set; }
        public string Login { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Role { get; set; } = "";
    }
}
