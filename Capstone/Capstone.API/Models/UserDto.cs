using Capstone.API.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.API.Models
{
    public class UserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string[] Properties { get; set; }
        public IEnumerable<Showing> ScheduledShowings { get; set; } //this is where the database will store these objects.

    }
}
