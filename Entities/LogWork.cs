using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class LogWork
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        [Required]
        [MaxLength(250)]
        public int ProjectId { get; set; }
        [Required]
        public DateTime? Date { get; set; }
        [Required]
        public int PhaseId { get; set; }
        [Required]
        public string Task { get; set; }
        [Required]
        public string From { get; set; }
        [Required]
        public string To { get; set; }
        [Required]
        [MaxLength(250)]
        public string Description { get; set; }

        [Required]
        public bool active { get; set; }
        [Required]
        public DateTime createdDate { get; set; }
        [Required]
        public DateTime updatedDate { get; set; }
    }
}
