using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public class PhaseDTO
    {
        public int? Id { get; set; } = null!;
        public string Name { get; set; }
        public string Description { get; set; }

        public string ValidateInput(bool IsUpdate)
        {
            if (String.IsNullOrEmpty(Name))
                return "Name is required!";
            if (Name.Length > 250)
                return "Length of Name must be <= 250 characters!";
            if (String.IsNullOrEmpty(Description))
                return "Description is required!";
            if (Description.Length > 250)
                return "Length of Description must be <= 250 characters!";
            return null;
        }
    }
}
