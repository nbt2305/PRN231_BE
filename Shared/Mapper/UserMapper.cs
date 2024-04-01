using Entities;
using Shared.DTO;
using Shared.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Mapper
{
    public class UserMapper
    {
        public static Users mapToUser(UserDTO request, Department department, JobPosition jobPosition) 
        {
            return new Users
            {
                UserName = request.UserName,
                FullName = request.FullName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Password = PasswordEncoder.Encode(request.Password),
                active = true,
                Department = department,
                JobPosition = jobPosition,
                UserType = request.UserType,
            };
        }

        public static UserDTO mapToUserDTO(Users request)
        {
            return new UserDTO
            {
                Id = request.Id,
                UserName = request.UserName,
                FullName = request.FullName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Password = request.Password,
                UserType = request.UserType,
                //DepartmentId = request.Department.Id,
                //JobPositionId = request.JobPosition.Id,
            };
        }

        public static Users mapToUserForUpdate(UserDTO request, Users response, Department department, JobPosition jobPosition)
        {
            response.UserName = request.UserName;
            response.FullName = request.FullName;
            response.Email = request.Email;
            response.PhoneNumber = request.PhoneNumber;
            response.Password = PasswordEncoder.Encode(request.Password);
            response.active = true;
            response.Department = department;
            response.JobPosition = jobPosition;
            response.UserType = request.UserType;
            response.updatedDate = DateTime.Now;
            return response;
        }
    }
}
