using Entities;
using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IJobPositionService : IBaseService<JobPositionDTO>
    {
        ResponseData<List<JobPositionDTO>> Search(string token, JobPositionDTO request);
    }
}
