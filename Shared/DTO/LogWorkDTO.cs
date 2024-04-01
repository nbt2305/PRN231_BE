using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public class LogWorkDTO
    {
        public int? UserId { get; set; }
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int PhaseId { get; set; }
        public string? Task { get; set; }
        public DateTime? Date { get; set; }
        public string? From { get; set; }
        public string? To { get; set; }
        public string Description { get; set; }
        public string ValidateInput(bool IsUpdate)
        {
           if (String.IsNullOrEmpty(Task))
                return "Task is required!";
            if (String.IsNullOrEmpty(From))
                return "From is required!";
            if (String.IsNullOrEmpty(To))
                return "To is required!";
            if (String.IsNullOrEmpty(Description))
                return "Description is required!";
            if (Description.Length > 250)
                return "Length of Description must be <= 8 characters!";
            if (checkTimeInput(From, To))
                return "Time End must be greater than Start!";
            return null;
        }
        public bool checkTimeInput(string from, string to)
        {
            int fromTime = int.Parse(from.Replace(":", ""));
            int toTime = int.Parse(to.Replace(":", ""));
            return (fromTime >= toTime);
        }
    }
}
