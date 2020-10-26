using Capstone.API.Models.Showing;

namespace Capstone.API.Models
{
    public class UnformattedShowingDto : OutboundShowingDto
    {
        public string RealtorID { get; set; }
        public string ProspectID { get; set; }
    }
}
