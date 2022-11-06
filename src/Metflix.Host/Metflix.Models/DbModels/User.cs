using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metflix.Models.DbModels
{
    public class UserInfo :BaseModel<string>
    {
        public UserInfo()
        {
            Id=Guid.NewGuid().ToString();
        }
        public string Name { get; init; } = null!;
        public DateTime DateOfBirth { get; init; }
        public string Email { get; init; } = null!;
        public string Password { get; set; } = null!;
        public DateTime CreatedDate { get; init; }       

        public string Role { get; init; } = null!;

    }
}
