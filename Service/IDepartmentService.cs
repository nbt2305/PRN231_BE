using Entities;
using Shared.DTO;
using Shared.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IDepartmentService : IBaseService<DepartmentDTO>
    {
        ResponseData<DepartmentResponse> GetPositionsByDepartment(string token, int departmentId);
        ResponseData<List<DepartmentDTO>> Search(string token, DepartmentDTO request);
    }
}
