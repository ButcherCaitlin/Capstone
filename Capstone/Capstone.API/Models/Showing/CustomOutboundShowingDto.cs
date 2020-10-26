using Capstone.API.Models.Showing;

namespace Capstone.API.Models
{
    public abstract class CustomOutboundShowingDto : OutboundShowingDto
    {

        public string CounterpartID { get; set; }
        public bool Host { get; set; }
    }
}
