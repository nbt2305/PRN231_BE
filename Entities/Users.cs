using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Users 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(250)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(250)]
        public string FullName { get; set; }
        [Required]
        [MaxLength(250)]
        public string UserType { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public bool active { get; set; }
        [Required]
        public DateTime createdDate { get; set; }
        [Required]
        public DateTime updatedDate { get; set; }
        public virtual Department Department { get; set; } = null!;
        public virtual JobPosition JobPosition { get; set; } = null!;
    }
}
