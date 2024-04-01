using Entities;
using Microsoft.Extensions.Options;
using Repository;
using Service.Telegram;
using Shared.Contants;
using Shared.DTO;
using Shared.DTO.Response;
using Shared.Mapper;
using Shared.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class DepartmentService : IDepartmentService
    {

        private readonly RepositoryContext _context;
        public DepartmentService(RepositoryContext context)
        {
            _context = context;
        }


        public ResponseData<DepartmentDTO> CreateNew(string token, DepartmentDTO request)
        {
            string errors = request.ValidateInput(false);

            if (errors != null)
            {
                return new ResponseData<DepartmentDTO>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrMsg = errors
                };
            }
            try
            {
                Department foundDepartment = _context.Departments.FirstOrDefault(x => x.Name == request.Name);
                if (foundDepartment != null)
                {
                    return new ResponseData<DepartmentDTO>
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        ErrMsg = "Department already exited!"
                    };
                }

                var department = DepartmentMapper.mapToDepartment(request);
                department.active = true;
                department.createdDate = DateTime.Now;
                _context.Departments.Add(department);
                _context.SaveChanges();

                List<Int32> jobPositions = request.JobPositions;
                List<DepartmentJobPosition> jobPositionSave = createOrUpdateDepartment(Constants.TYPE_CREATE, jobPositions, null, department.Id);
                _context.DepartmentJobPositions.AddRange(jobPositionSave);
                _context.SaveChanges();

                var telegram = new TelegramService("7125536679:AAF6Iu95kP0AnxanPVl2-9pXOspA37NbUBA");
                var message = "Bạn đã tạo thành công Department mới với Name là " + department.Name;
                telegram.SendMessageAsync("1192099901", message);
                return new ResponseData<DepartmentDTO>
                {
                    Data = DepartmentMapper.mapToDepartmentDTO(department),
                    StatusCode = HttpStatusCode.OK,
                };
                
            }
            catch (Exception ex)
            {
                return new ResponseData<DepartmentDTO>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrMsg = "Created Error"
                };
            }
        }

        public List<DepartmentJobPosition> createOrUpdateDepartment(string type, List<int> requestCreate, List<int> requestUpdate, int departmentId)
        {
            if (type.Equals(Constants.TYPE_CREATE))
            {
                List<DepartmentJobPosition> response = new List<DepartmentJobPosition>();
                foreach (int jobPositionId in requestCreate)
                {
                    DepartmentJobPosition departmentJobPosition = new DepartmentJobPosition
                    {
                        DepartmentId = departmentId,
                        JobPositionId = jobPositionId
                    };
                    response.Add(departmentJobPosition);
                }
                return response;
            }
            else
            {
                var foundDepartmentJobPositions = _context.DepartmentJobPositions.Where(x => x.DepartmentId == departmentId).ToList();
                _context.RemoveRange(foundDepartmentJobPositions);
                _context.SaveChanges();
                List<DepartmentJobPosition> response = new List<DepartmentJobPosition>();
                foreach (int jobPositionId in requestUpdate)
                {
                    DepartmentJobPosition departmentJobPosition = new DepartmentJobPosition 
                    {
                        DepartmentId = departmentId,
                        JobPositionId = jobPositionId
                    };
                    response.Add(departmentJobPosition);
                }
                return response;
            }
        }

        public ResponseData<DepartmentDTO> Delete(string token, int Id)
        {
            Department foundDepartment = _context.Departments.FirstOrDefault(x => x.Id == Id);
            if (foundDepartment == null)
            {
                return new ResponseData<DepartmentDTO>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrMsg = "Department not found!"
                };
            }

            var user = _context.Users.FirstOrDefault(x => x.Department.Id == Id);
            if (user != null)
            {
                return new ResponseData<DepartmentDTO>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrMsg = "Department is being use!"
                };
            }

            var departmentJobPositions = _context.DepartmentJobPositions.Where(x => x.DepartmentId == Id).ToList();
            _context.DepartmentJobPositions.RemoveRange(departmentJobPositions);
            _context.SaveChanges();

            var response = new ResponseData<DepartmentDTO>
            {
                Data = DepartmentMapper.mapToDepartmentDTO(foundDepartment),
                StatusCode = HttpStatusCode.OK,
            };

            _context.Departments.Remove(foundDepartment);
            _context.SaveChanges();

            return response;
        }

        public ResponseData<DepartmentDTO> GetById(string token, int Id)
        {
            var foundDepartment = _context.Departments.FirstOrDefault(x => x.Id == Id);
            if (foundDepartment == null)
            {
                return new ResponseData<DepartmentDTO>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrMsg = "Department not found!"
                };
            }

            var jobPositions = new List<int>();

            var departmentJobPosition = _context.DepartmentJobPositions.Where(x => x.DepartmentId == Id).ToList();
            if(departmentJobPosition != null)
            {
                jobPositions = departmentJobPosition.Select(x => x.JobPositionId).ToList();
            }

            var result = DepartmentMapper.mapToDepartmentDTO(foundDepartment);
            result.JobPositions = jobPositions;

            var response = new ResponseData<DepartmentDTO>
            {
                Data = result,
                StatusCode = HttpStatusCode.OK,
            };

            return response;
        }

        public ResponseData<List<DepartmentDTO>> GetList(string token)
        {
            throw new NotImplementedException();
        }

        public ResponseData<DepartmentResponse> GetPositionsByDepartment(string token, int departmentId)
        {
            DepartmentResponse response = new DepartmentResponse();
            Department foundDepartment = _context.Departments.FirstOrDefault(x => x.Id == departmentId);
            if (foundDepartment == null)
            {
                return new ResponseData<DepartmentResponse>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrMsg = "Department not found!"
                };
            }
            response.Id = foundDepartment.Id;
            response.Name = foundDepartment.Name;
            response.Description = foundDepartment.Description;
            List<JobPosition> jobPositions= new List<JobPosition>();
            List<DepartmentJobPosition> departmentJobPositions = _context.DepartmentJobPositions.Where(x => x.DepartmentId == departmentId).ToList();
            foreach (DepartmentJobPosition departmentJobPosition in departmentJobPositions)
            {
                JobPosition jobPosition = _context.JobPositions.FirstOrDefault(x => x.Id == departmentJobPosition.JobPositionId);
                if (jobPosition != null)
                {
                    jobPositions.Add(jobPosition);
                }
            }

            response.JobPositions = jobPositions;

            return new ResponseData<DepartmentResponse>
            {
                Data = response,
                StatusCode = HttpStatusCode.OK,
            };
        }

        public ResponseData<List<DepartmentDTO>> Search(string token, DepartmentDTO request)
        {
            var list = _context.Departments.Where(x => 
            (x.Name.ToLower().Contains(request.Name.ToLower()) || request.Name == null) &&
            (x.Description.ToLower().Contains(request.Description.ToLower()) || request.Description == null))
                .ToList();
            var data = list.Select(u => DepartmentMapper.mapToDepartmentDTO(u)).ToList();

            return new ResponseData<List<DepartmentDTO>>()
            {
                Data = data,
                StatusCode = HttpStatusCode.OK,
            };
        }

        public ResponseData<DepartmentDTO> Update(string token, int id, DepartmentDTO request)
        {
            string errors = request.ValidateInput(false);

            if (errors != null)
            {
                return new ResponseData<DepartmentDTO>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrMsg = errors
                };
            }

            try
            {
                Department foundDepartment = _context.Departments.FirstOrDefault(x => x.Id == id);
                if (foundDepartment == null)
                {
                    return new ResponseData<DepartmentDTO>
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        ErrMsg = "Department not found!"
                    };
                }

                if ((foundDepartment.Name != request.Name) &&
                        (_context.Departments.FirstOrDefault(x => x.Name == request.Name) != null))
                {
                    return new ResponseData<DepartmentDTO>
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        ErrMsg = "Department Name already existed!"
                    };
                }

                foundDepartment = DepartmentMapper.mapToDepartmentForUpdate(request, foundDepartment);
                _context.Departments.Update(foundDepartment);
                _context.SaveChanges();


                List<Int32> jobPositions = request.JobPositions;
                List<DepartmentJobPosition> jobPositionSave = createOrUpdateDepartment(Constants.TYPE_UPDATE, null, jobPositions, foundDepartment.Id);
                _context.DepartmentJobPositions.AddRange(jobPositionSave);
                _context.SaveChanges();

                return new ResponseData<DepartmentDTO>
                {
                    Data = DepartmentMapper.mapToDepartmentDTO(foundDepartment),
                    StatusCode = HttpStatusCode.OK,
                };

            }
            catch (Exception ex)
            {
                return new ResponseData<DepartmentDTO>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrMsg = "Updated Error"
                };
            }
        }
    }
}
