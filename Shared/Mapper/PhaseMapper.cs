using Entities;
using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Mapper
{
    public class PhaseMapper
    {
        public static Phase mapToPhase(PhaseDTO request)
        {
            return new Phase
            {
                Name = request.Name,
                Description = request.Description,
                createdDate = DateTime.Now,
            };
        }

        public static PhaseDTO mapToPhaseDTO(Phase request)
        {
            return new PhaseDTO
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
            };
        }

        public static Phase mapToPhaseForUpdate(PhaseDTO request, Phase response)
        {
            response.Name = request.Name;
            response.Description = request.Description;
            response.updatedDate = DateTime.Now;
            return response;
        }
    }
}
