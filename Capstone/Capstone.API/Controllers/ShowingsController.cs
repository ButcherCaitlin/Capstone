using AutoMapper;
using Capstone.API.Models;
using Capstone.API.Models.Showing;
using Capstone.API.ResourceParameters;
using Capstone.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.API.Controllers
{
    [Route("api/showings")]
    [ApiController]
    public class ShowingsController : ControllerBase
    {
        private readonly ShowingService _showingService;
        private readonly IMapper _mapper;

        public ShowingsController(ShowingService showingService,
            IMapper mapper)
        {
            _showingService = showingService ??
                throw new ArgumentNullException(nameof(showingService));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Returns a set of showings based on a set of filtering and search parameters sepecified in the query string.
        /// </summary>
        /// <param name="parameters">The the filtering and searching parameters.</param>
        /// <returns>The set of showings specified by the parameters.</returns>
        [HttpGet]
        [HttpHead]
        public ActionResult<IEnumerable<OutboundShowingDto>> GetShowings(
            [FromQuery] ShowingsResourceParameters parameters = null)
        {
            var showingsFromRepo = _showingService.Get(parameters);
            if (showingsFromRepo == null) return NotFound();

            return Ok(_mapper.Map<IEnumerable<UnformattedShowingDto>>(showingsFromRepo));
        }
        /// <summary>
        /// Returns a showing with the ID provided in the route.
        /// </summary>
        /// <param name="propertyId">The ID of the showing to be found. Provided in the route.</param>
        /// <returns>The showing object.</returns>
        [HttpGet("{showingId}", Name = "GetShowingById")]
        [HttpHead("{showingId}")]
        public ActionResult<CustomOutboundShowingDto> GetShowingById(string showingId,
            [FromHeader] string userId = null)
        {
            var showingFromRepo = _showingService.Get(showingId);
            if (showingFromRepo == null) return NotFound();

            if (showingFromRepo.ProspectID == userId)
            {
                return Ok(_mapper.Map<ProspectShowingDto>(showingFromRepo));
            }
            else if (showingFromRepo.RealtorID == userId)
            {
                return Ok(_mapper.Map<RealtorShowingDto>(showingFromRepo));
            }
            else
            {
                return Ok(_mapper.Map<UnformattedShowingDto>(showingFromRepo));
            }
        }

        [HttpOptions]
        public IActionResult GetShowingsOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,HEAD");
            return Ok();
        }
    }
}
