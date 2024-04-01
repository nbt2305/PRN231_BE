using Entities;
using Shared.DTO;
using Shared.DTO.Mail;
using Shared.DTO.Request;
using Shared.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IUserService : IBaseService<UserDTO>
    {
        ResponseData<UserWithToken> Login(LoginRequest request);
        ResponseData<List<UserDTO>> Search(string token, UserDTO request);
        ResponseData<ProjectsOfUser> GetProjectOfUser(string token, int userId);
        int GetUserIdFromToken(string token);
    }
}
