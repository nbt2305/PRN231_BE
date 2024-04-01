using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO.Response
{
    public class UserInProjectResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public JobPositionDTO jobPosition { get; set; }
        public LevelDTO level { get; set; }
    }

}
