using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO.Request
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public string ValidateInput()
        {
            if (String.IsNullOrEmpty(UserName))
                return "UserName is required!";
            if (String.IsNullOrEmpty(Password))
                return "Password is required!";
            if (Password.Length < 8)
                return "Length of Password must be >= 8 characters!";
            return null;
        }
    }
}
