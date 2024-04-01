using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO.Response
{
    public class ProjectsOfUser
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public List<ProjectForUser> Projects { get; set; }
    }

    public class ProjectForUser
    {
        public int Id { get; set; }
        public string projectCode { get; set; }
        public string projectName { get; set; }
    }
}
