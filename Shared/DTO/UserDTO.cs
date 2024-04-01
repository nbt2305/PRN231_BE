using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public class UserDTO
    {
        public int? Id { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public int? DepartmentId { get; set; }
        public int? JobPositionId { get; set; }
        public string? Password { get; set; }
        public string? UserType { get; set; }

        public string ValidateInput(bool IsUpdate)
        {
            if (String.IsNullOrEmpty(FullName))
                return "FullName is required!";
            if (FullName.Length > 250)
                return "Length of FullName must be <= 250 characters!";
            if (String.IsNullOrEmpty(UserType))
                return "UserType is required!";
            return null;
        }
    }
}
