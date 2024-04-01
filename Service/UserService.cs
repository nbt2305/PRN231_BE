using AutoMapper;
using Entities;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MimeKit.Text;
using MimeKit;
using Repository;
using Shared.DTO;
using Shared.DTO.Request;
using Shared.Mapper;
using Shared.Security;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Shared.DTO.Mail;
using Shared.DTO.Response;

namespace Service
{
    public class UserService : IUserService
    {
        private readonly RepositoryContext _context;
        private readonly JWTSettings _jwtsettings;
        private readonly TokenService _tokenService;



        public UserService(RepositoryContext context, IOptions<JWTSettings> jwtsettings)
        {
            _context = context;
            _jwtsettings = jwtsettings.Value;
        }
        public ResponseData<UserDTO> CreateNew(string token, UserDTO request)
        {

            string errors = request.ValidateInput(false);

            if (errors != null)
            {
                return new ResponseData<UserDTO>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrMsg = errors
                };
            }
            try
            {
                Department department = _context.Departments.FirstOrDefault(x => x.Id == request.DepartmentId);
                if (department == null)
                {
                    return new ResponseData<UserDTO>
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        ErrMsg = "Department not found!"
                    };
                }

                JobPosition jobPosition = _context.JobPositions.FirstOrDefault(x => x.Id == request.JobPositionId);
                if (jobPosition == null)
                {
                    return new ResponseData<UserDTO>
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        ErrMsg = "JobPosition not found!"
                    };
                }

                Users foundUser = _context.Users.FirstOrDefault(x => x.UserName== request.UserName);
                if (foundUser != null)
                {
                    return new ResponseData<UserDTO>
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        ErrMsg = "User name already exited!"
                    };
                }

                Users foundUserByEmail = _context.Users.FirstOrDefault(x => x.Email == request.Email);
                if (foundUser != null)
                {
                    return new ResponseData<UserDTO>
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        ErrMsg = "Email already exited!"
                    };
                }


                Users users = UserMapper.mapToUser(request, department, jobPosition);
                users.createdDate= DateTime.Now;
                _context.Users.Add(users);
                _context.SaveChanges();
                return new ResponseData<UserDTO>
                {
                    Data = UserMapper.mapToUserDTO(users),
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<UserDTO>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrMsg = "Created Error"
                };
            }

            
        }

        public ResponseData<UserDTO> Delete(string token, int Id)
        {
            throw new NotImplementedException();
        }

        public ResponseData<UserDTO> GetById(string token, int Id)
        {
            var foundUser = _context.Users.FirstOrDefault(x => x.Id == Id);
            if (foundUser == null)
            {
                return new ResponseData<UserDTO>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ErrMsg = "User not found!"
                };
            }

            return new ResponseData<UserDTO>
            {
                Data = UserMapper.mapToUserDTO(foundUser),
                StatusCode = HttpStatusCode.OK,
            };
        }

        public ResponseData<List<UserDTO>> GetList(string token)
        {
            var res = _context.Users.ToList();
            var result = res.Select(x => UserMapper.mapToUserDTO(x)).ToList();
            return new ResponseData<List<UserDTO>>
            {
                Data = result,
                StatusCode = HttpStatusCode.OK,
            };
        }

        public int GetUserIdFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("$2a$12$atfA/R0/eug64md1RHG.ROPhIUBlQrZy2Ags9Dx1O6xm9nAm/LRcG");

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // Set clock skew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

                if (validatedToken is not JwtSecurityToken jwtToken)
                    throw new SecurityTokenException("Invalid token");

                var userIdClaim = principal.FindFirst(ClaimTypes.Name);
                if (userIdClaim != null)
                {
                    var userId = userIdClaim.Value;
                    return Convert.ToInt32(userId);
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                Console.WriteLine($"Exception: {ex.Message}");
            }
            return 0;
        }


        // Generate Access Token
        private string GenerateAccessToken(Users user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            _jwtsettings.SecretKey = "$2a$12$atfA/R0/eug64md1RHG.ROPhIUBlQrZy2Ags9Dx1O6xm9nAm/LRcG";
            var key = Encoding.ASCII.GetBytes(_jwtsettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, Convert.ToString(user.Id)),
                    new Claim(ClaimTypes.Role, Convert.ToString(user.UserType))
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public ResponseData<UserWithToken> Login(LoginRequest request)
        {
            string errors = request.ValidateInput();

            if (errors != null)
            {
                return new ResponseData<UserWithToken>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrMsg = errors
                };
            }

            try
            {
                Users foundUser = _context.Users.FirstOrDefault(x => x.UserName == request.UserName);
                Users foundUserByEmail = _context.Users.FirstOrDefault(x => x.Email == request.UserName);
                if (foundUser == null && foundUserByEmail == null)
                {
                    return new ResponseData<UserWithToken>
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        ErrMsg = "Username or password incorrect!"
                    };
                }

                UserWithToken userWithToken = new UserWithToken();

                if (foundUser != null)
                {
                    if (!PasswordEncoder.matches(request.Password, foundUser.Password))
                    {
                        return new ResponseData<UserWithToken>
                        {
                            StatusCode = HttpStatusCode.InternalServerError,
                            ErrMsg = "Username or password incorrect!"
                        };
                    }
                    userWithToken = new UserWithToken(foundUser);
                    userWithToken.AccessToken = GenerateAccessToken(foundUser);
                }

                if(foundUserByEmail != null)
                {
                    if (!PasswordEncoder.matches(request.Password, foundUserByEmail.Password))
                    {
                        return new ResponseData<UserWithToken>
                        {
                            StatusCode = HttpStatusCode.InternalServerError,
                            ErrMsg = "Username or password incorrect!"
                        };
                    }
                    userWithToken = new UserWithToken(foundUserByEmail);
                    userWithToken.AccessToken = GenerateAccessToken(foundUserByEmail);
                }


                return new ResponseData<UserWithToken>
                {
                    Data = userWithToken,
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<UserWithToken>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrMsg = "Login failed!"
                };
            }
        }

        public ResponseData<UserDTO> Update(string token, int id, UserDTO request)
        {
            string errors = request.ValidateInput(false);

            if (errors != null)
            {
                return new ResponseData<UserDTO>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrMsg = errors
                };
            }

            try
            {
                var foundUser = _context.Users.FirstOrDefault(x => x.Id == id);
                var currentUserId = GetUserIdFromToken(token);
                var currentUser = _context.Users.FirstOrDefault(x => x.Id == currentUserId);
                if (currentUser.Id != id)
                {
                    return new ResponseData<UserDTO>()
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        ErrMsg = "Permisson Denied!"
                    };
                }

                Department department = _context.Departments.FirstOrDefault(x => x.Id == request.DepartmentId);
                if (department == null)
                {
                    return new ResponseData<UserDTO>
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        ErrMsg = "Department not found!"
                    };
                }

                JobPosition jobPosition = _context.JobPositions.FirstOrDefault(x => x.Id == request.JobPositionId);
                if (jobPosition == null)
                {
                    return new ResponseData<UserDTO>
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        ErrMsg = "JobPosition not found!"
                    };
                }

                if(foundUser != null)
                {
                    if((foundUser.UserName != request.UserName) && 
                        (_context.Users.FirstOrDefault(x => x.UserName == request.UserName) != null))
                    {
                        return new ResponseData<UserDTO>
                        {
                            StatusCode = HttpStatusCode.InternalServerError,
                            ErrMsg = "User Name already existed!"
                        };
                    }
                    foundUser = UserMapper.mapToUserForUpdate(request, foundUser, department, jobPosition);
                    _context.SaveChanges();
                    return new ResponseData<UserDTO>
                    {
                        Data = UserMapper.mapToUserDTO(foundUser),
                        StatusCode = HttpStatusCode.InternalServerError
                    };
                }
                return new ResponseData<UserDTO>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrMsg = "User not found!"
                };
            }
            catch(Exception ex)
            {
                return new ResponseData<UserDTO>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrMsg = "Update failed!"
                };
            }
        }

        public ResponseData<List<UserDTO>> Search(string token, UserDTO request)
        {
            var currentUserId = GetUserIdFromToken(token);
            //var currentUser = _context.Users.FirstOrDefault(x => x.Id == currentUserId);
            //if (currentUser.UserType != "ADMIN")
            //{
            //    return new ResponseData<List<UserDTO>>()
            //    {
            //        StatusCode = HttpStatusCode.InternalServerError,
            //        ErrMsg = "Permisson Denied!"
            //    };
            //}
            var list = _context.Users.Where(x =>
                (request.FullName == "" || x.FullName.ToLower().Contains(request.FullName.ToLower())) &&
                (request.UserType == "" || x.UserType == request.UserType) &&
                (request.DepartmentId == 0 || x.Department.Id == request.DepartmentId) &&
                (request.JobPositionId == 0 || x.JobPosition.Id == request.JobPositionId)
            ).ToList();
            var data = list.Select(u => UserMapper.mapToUserDTO(u)).ToList();
            return new ResponseData<List<UserDTO>>()
            {
                Data = data,
                StatusCode = HttpStatusCode.OK,
            };
        }

        public ResponseData<ProjectsOfUser> GetProjectOfUser(string token, int userId)
        {
            var foundUser = _context.Users.FirstOrDefault(x => x.Id == userId);

            if (foundUser == null)
            {
                return new ResponseData<ProjectsOfUser>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ErrMsg = "User not found!"
                };
            };

            var projectIds = _context.ProjectUsers.Where(x => x.UserId == userId)
                .Select(x => x.ProjectId).ToList();
            var projects = new List<Project>();

            foreach (var projectId in projectIds)
            {
                var foundProject = _context.Projects.FirstOrDefault(x => x.Id == projectId);
                projects.Add(foundProject);
            }

            var projectForUser = projects.Select(x => new ProjectForUser()
            {
                projectCode = x.ProjectCode,
                projectName = x.ProjectName,
                Id = x.Id
            }).ToList();

            var result = new ProjectsOfUser();
            result.Projects = projectForUser;
            result.Id = userId;
            result.FullName = foundUser.FullName;

            return new ResponseData<ProjectsOfUser>
            {
                Data = result,
                StatusCode= HttpStatusCode.OK,
            };
        }
    }
}
