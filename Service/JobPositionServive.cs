using Entities;
using Microsoft.EntityFrameworkCore;
using Repository;
using Shared.DTO;
using Shared.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class JobPositionServive : IJobPositionService
    {
        private readonly RepositoryContext _context;
        public JobPositionServive(RepositoryContext context)
        {
            _context = context;
        }

        public ResponseData<JobPositionDTO> CreateNew(string token, JobPositionDTO request)
        {
            string errors = request.ValidateInput(false);

            if (errors != null)
            {
                return new ResponseData<JobPositionDTO>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrMsg = errors
                };
            }
            try
            {
                var foundJobPosition = _context.JobPositions.FirstOrDefault(x => x.Name == request.Name);
                if (foundJobPosition != null)
                {
                    return new ResponseData<JobPositionDTO>
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        ErrMsg = "Job Position already exited!"
                    };
                }

                var jobPosition = JobPositionMapper.mapToJobPosition(request);
                jobPosition.active = true;
                jobPosition.createdDate = DateTime.Now;
                _context.JobPositions.Add(jobPosition);
                _context.SaveChanges();


                return new ResponseData<JobPositionDTO>
                {
                    Data = JobPositionMapper.mapToJobPositionDTO(jobPosition),
                    StatusCode = HttpStatusCode.OK,
                };

            }
            catch (Exception ex)
            {
                return new ResponseData<JobPositionDTO>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrMsg = "Created Error"
                };
            }
        }

        // Handle case nếu như JobPosition đang được gán cho một user khác
        public ResponseData<JobPositionDTO> Delete(string token, int Id)
        {
            var foundJobPosition = _context.JobPositions.Include(x => x.Userss).FirstOrDefault(x => x.Id == Id);
            if (foundJobPosition == null)
            {
                return new ResponseData<JobPositionDTO>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrMsg = "Job Position not found!"
                };
            }

            var users = foundJobPosition.Userss;
            if (users.Count > 0)
            {
                return new ResponseData<JobPositionDTO>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrMsg = "Can not deleted!"
                };
            }

            var response = new ResponseData<JobPositionDTO>
            {
                Data = JobPositionMapper.mapToJobPositionDTO(foundJobPosition),
                StatusCode = HttpStatusCode.OK,
            };

            _context.JobPositions.Remove(foundJobPosition);
            _context.SaveChanges();

            return response;

        }

        public ResponseData<List<JobPositionDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public ResponseData<JobPositionDTO> GetById(string token, int Id)
        {
            var response = _context.JobPositions.FirstOrDefault(x => x.Id == Id);
            if (response == null)
            {
                return new ResponseData<JobPositionDTO>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrMsg = "Job Position not found!"
                };
            }

            return new ResponseData<JobPositionDTO>
            {
                Data = JobPositionMapper.mapToJobPositionDTO(response),
                StatusCode = HttpStatusCode.OK,
            };
        }

        public ResponseData<List<JobPositionDTO>> GetList(string token)
        {
            var list = _context.JobPositions.ToList();
            var result = list.Select(x => JobPositionMapper.mapToJobPositionDTO(x)).ToList();
            return new ResponseData<List<JobPositionDTO>>
            {
                Data = result,
                StatusCode = HttpStatusCode.OK,
            };
        }

        public ResponseData<List<JobPositionDTO>> Search(string token, JobPositionDTO request)
        {
            var list = _context.JobPositions.Where(x =>
            (x.Name.ToLower().Contains(request.Name.ToLower()) || request.Name == null) &&
            (x.Description.ToLower().Contains(request.Description.ToLower()) || request.Description == null))
                .ToList();
            var data = list.Select(u => JobPositionMapper.mapToJobPositionDTO(u)).ToList();

            return new ResponseData<List<JobPositionDTO>>()
            {
                Data = data,
                StatusCode = HttpStatusCode.OK,
            };
        }

        public ResponseData<JobPositionDTO> Update(string token, int id, JobPositionDTO request)
        {
            string errors = request.ValidateInput(false);

            if (errors != null)
            {
                return new ResponseData<JobPositionDTO>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrMsg = errors
                };
            }

            try
            {
                var foundJobPosition = _context.JobPositions.FirstOrDefault(o => o.Id == id);
                if (foundJobPosition == null)
                {
                    return new ResponseData<JobPositionDTO>
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        ErrMsg = "Job Position not found!"
                    };
                }

                if (foundJobPosition.Name != request.Name &&
                        _context.JobPositions.FirstOrDefault(x => x.Name == request.Name) != null)
                {
                    return new ResponseData<JobPositionDTO>
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        ErrMsg = "JobPosition Name already existed!"
                    };
                }
                foundJobPosition = JobPositionMapper.mapToJobPositionForUpdate(request, foundJobPosition);
                _context.JobPositions.Update(foundJobPosition);
                _context.SaveChanges();

                return new ResponseData<JobPositionDTO>
                {
                    Data = JobPositionMapper.mapToJobPositionDTO(foundJobPosition),
                    StatusCode = HttpStatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<JobPositionDTO>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrMsg = "Updated Error"
                };
            }


        }
    }
}
