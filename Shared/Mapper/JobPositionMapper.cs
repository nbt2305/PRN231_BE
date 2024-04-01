using Entities;
using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Mapper
{
    public class JobPositionMapper
    {
        public static JobPosition mapToJobPosition(JobPositionDTO request)
        {
            return new JobPosition
            {
                Name = request.Name,
                Description = request.Description,
            };
        }

        public static JobPositionDTO mapToJobPositionDTO(JobPosition request)
        {
            return new JobPositionDTO
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
            };
        }

        public static JobPosition mapToJobPositionForUpdate(JobPositionDTO request, JobPosition response)
        {
            response.Name = request.Name;
            response.Description = request.Description;
            response.updatedDate = DateTime.Now;
            return response;
        }
    }
}
