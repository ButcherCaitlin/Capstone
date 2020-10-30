using AutoMapper;
using Capstone.API.Entities;
using Capstone.API.Models;
using Capstone.API.ResourceParameters;
using Capstone.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Core.Operations;
using System;
using System.Collections.Generic;

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
        [HttpGet("{showingId:length(24)}", Name = "GetShowingById")]
        [HttpHead("{showingId:length(24)}")]
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

        [HttpPut("{showingId:length(24)}")]
        public IActionResult UpdateShowing(string showingId, UpdateShowingDto showing,
            [FromHeader] string userId = null)
        {
            if (userId == null) return BadRequest(new { message = "A UserID is required to Modify showing records." });

            var showingFromRepo = _showingService.Get(showingId);
            if (showingFromRepo == null) return NotFound();

            if (showingFromRepo.ProspectID != userId ||
                showingFromRepo.RealtorID != userId)
            {
                return BadRequest(new { message = "You can only modify showing records if you are a participant." });
            }
            _mapper.Map(showing, showingFromRepo);
            _showingService.Update(showingFromRepo.Id, showingFromRepo);
            return NoContent();
        }

        [HttpPatch("{showingId:length(24)}")]
        public IActionResult PartiallyUpdateShowing(string showingId,
            JsonPatchDocument<UpdateShowingDto> patchDocument,
            [FromHeader] string userId = null)
        {
            if (userId == null) return BadRequest(new { message = "A UserID is required to Modify showing records." });

            var showingToPatch = _showingService.Get(showingId);
            if (showingToPatch == null) return NotFound(new { message = "If you are trying to upsert a resource use PUT" });


            if (showingToPatch.ProspectID != userId &&
                showingToPatch.RealtorID != userId)
            {
                return BadRequest(new { message = "You can only modify showing records if you are a participant." });
            }

            var showingDtoToPatch = _mapper.Map<UpdateShowingDto>(showingToPatch);
            patchDocument.ApplyTo(showingDtoToPatch, ModelState);

            if (!TryValidateModel(showingDtoToPatch)) return ValidationProblem(ModelState);

            _mapper.Map(showingDtoToPatch, showingToPatch);
            _showingService.Update(showingToPatch.Id, showingToPatch);

            return NoContent();
        }

        [HttpDelete("{showingId}")]
        public IActionResult DeleteShowing(string showingId,
            [FromHeader] string userId = null)
        {
            if (userId == null) return BadRequest(new { message = "A UserID is required to delete showing records." });
            var showingToDelete = _showingService.Get(showingId);
            if (showingToDelete == null) return NotFound();
            if (showingToDelete.ProspectID != userId &&
                showingToDelete.RealtorID != userId)
            {
                return BadRequest(new { message = "You can only delete showing records if you are a participant." });
            }
            _showingService.Remove(showingToDelete.Id);
            return NoContent();
        }

        [HttpOptions]
        public IActionResult GetShowingsOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,HEAD");
            return Ok();
        }
    }
}
