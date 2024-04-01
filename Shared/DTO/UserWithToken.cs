using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public class UserWithToken : Users
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public UserWithToken() { }

        public UserWithToken(Users user)
        {
            this.Id = user.Id;
            this.UserName = user.UserName;
            this.FullName = user.FullName;
            this.Email = user.Email;
            this.PhoneNumber = user.PhoneNumber;
            this.UserType = user.UserType;
            this.Password = user.Password;
            this.active = user.active;
        }
    }
}
