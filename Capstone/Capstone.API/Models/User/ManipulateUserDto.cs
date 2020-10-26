using System.ComponentModel.DataAnnotations;

namespace Capstone.API.Models
{
    public abstract class ManipulateUserDto
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(50)]
        public string Password { get; set; }
        [Phone]
        [MaxLength(50)]
        public string Phone { get; set; }
    }
}
