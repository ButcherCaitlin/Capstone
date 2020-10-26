using System.ComponentModel.DataAnnotations;

namespace Capstone.API.Models
{
    //you can add custom error messages, or class level validation with custom validationattributes.
    public class CreateUserDto 
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
