using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO.Request
{
    public class ProjectSearch
    {
        public string ProjectCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
