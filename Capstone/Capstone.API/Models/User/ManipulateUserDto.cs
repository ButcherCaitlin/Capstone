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
        [MinLength(8)]
        public string Password { get; set; }
        [Phone]
        [MaxLength(50)]
        public string Phone { get; set; }
        [Required]
        public string UserType { get; set; }

        public string AuthToken { get; set; }
        public string Id { get; set; }
        public string HashedPassword { get; set; }

    }
}
