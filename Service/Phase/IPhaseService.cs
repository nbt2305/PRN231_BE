using Entities;
using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Phases
{
    public interface IPhaseService : IBaseService<PhaseDTO>
    {
        ResponseData<List<PhaseDTO>> Search(string token, PhaseDTO request);
    }
}
