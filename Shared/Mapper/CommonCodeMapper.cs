using Entities;
using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Mapper
{
    public class CommonCodeMapper
    {
        public static LevelDTO mapToLevelDTO(CommonCode request)
        {
            return new LevelDTO
            {
                Id = request.Id,
                Type = request.Type,
                Value = request.Value,
            };
        }
    }
}
