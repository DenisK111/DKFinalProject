using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metflix.Models.Requests.Identity
{
    public class RegisterRequest
    {
        public string Name { get; set; } = null!; 
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
        public string DateOfBirth { get; set; } = null!;
    }
}
