using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Security
{
    public class PasswordEncoder
    {
        public static string Encode(string rawPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(rawPassword);
        }

        public static bool matches(string rawPassword, string encodedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(rawPassword, encodedPassword);
        }
    }
}
