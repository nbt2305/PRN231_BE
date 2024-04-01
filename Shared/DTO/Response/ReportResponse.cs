using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO.Response
{
    public class ReportResponse
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public int NumberOfUsers { get; set; }
    }
}
