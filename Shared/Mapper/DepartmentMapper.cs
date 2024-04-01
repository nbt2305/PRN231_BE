using Entities;
using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Mapper
{
    public class DepartmentMapper
    {
        public static Department mapToDepartment(DepartmentDTO request)
        {
            return new Department
            {
                Name = request.Name,
                Description = request.Description,
            };
        }

        public static DepartmentDTO mapToDepartmentDTO(Department request)
        {
            return new DepartmentDTO
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                CreatedAt = request.createdDate,
                UpdatedAt= request.updatedDate,
            };
        }

        public static Department mapToDepartmentForUpdate(DepartmentDTO request, Department response)
        {
            response.Name = request.Name;
            response.Description = request.Description;
            response.updatedDate = DateTime.Now;
            return response;
        }
    }
}
