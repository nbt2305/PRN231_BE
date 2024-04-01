using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO.Response
{
    public class ProjectResponse
    {
        public string? ProjectCode { get; set; }
        public string? ProjectName { get; set; }
        public string? Description { get; set; }
        public List<UserInProjectResponse> Users { get; set; }
        public List<int> Phases { get; set; }
    }
}
