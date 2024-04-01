using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public class ProjectDTO
    {
        public int? Id { get; set; }
        public string? ProjectCode { get; set; }
        public string? ProjectName { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public List<ProjectUserDTO> ProjectUsers { get; set; }
        public List<int> Phases { get; set; }

        public string ValidateInput(bool IsUpdate)
        {
            if (String.IsNullOrEmpty(ProjectCode))
                return "ProjectCode is required!";
            if (ProjectCode.Length > 250)
                return "Length of ProjectCode must be <= 250 characters!";
            if (String.IsNullOrEmpty(ProjectName))
                return "ProjectName is required!";
            if (ProjectName.Length > 250)
                return "Length of ProjectName must be <= 250 characters!";
            if (String.IsNullOrEmpty(Description))
                return "Description is required!";
            if (Description.Length > 250)
                return "Length of Description must be <= 8 characters!";
            return null;
        }
    }
}
