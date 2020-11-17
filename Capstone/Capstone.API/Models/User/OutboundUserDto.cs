namespace Capstone.API.Models
{
    public class OutboundUserDto
    {
        public string AuthToken { get; set; }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string UserType { get; set; }
        public string HashedPassword { get; set; }
    }
}
