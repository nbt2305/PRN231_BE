using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO.Response
{
    public class LogWorkResponse
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string PhaseName { get; set; }
        public string Task { get; set; }
        public DateTime? Date { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Description { get; set; }
    }
}
