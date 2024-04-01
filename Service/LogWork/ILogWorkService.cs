using Entities;
using Shared.DTO;
using Shared.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.LogWork
{
    public interface ILogWorkService : IBaseService<LogWorkDTO>
    {
        ResponseData<List<LogWorkResponse>> Search(string token, LogWorkDTO request);
    }
}
