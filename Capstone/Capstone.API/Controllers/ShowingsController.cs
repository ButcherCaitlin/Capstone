using AutoMapper;
using Capstone.API.Models;
using Capstone.API.ResourceParameters;
using Capstone.API.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.API.Controllers
{
    [Route("api/showings")]
    [ApiController]
    public class ShowingsController : ControllerBase
    {
        private readonly ShowingRepository _showingService;
        private readonly IMapper _mapper;
        public ShowingsController(ShowingRepository showingService,
            IMapper mapper)
        {
            _showingService = showingService ??
                throw new ArgumentNullException(nameof(showingService));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }
        /// <summary>
        /// Queries Showing records from the database based off of a set of query parameters.
        /// </summary>
        /// <param name="parameters">The filter and Search parameters. Found in the query string.</param>
        /// <returns>The records to be returned as UnformattedShowingDto's.</returns>
        [HttpGet]
        [HttpHead]
        public ActionResult<IEnumerable<OutboundShowingDto>> GetShowings(
            [FromQuery] ShowingsResourceParameters parameters = null,
            [FromHeader] string userId = null)
        {
            var showingsFromRepo = _showingService.Get(parameters);
            if (showingsFromRepo == null) return NotFound();

            List<OutboundShowingDto> showingsToReturn = new List<OutboundShowingDto>();

            showingsToReturn.AddRange(
                _mapper.Map<IEnumerable<ProspectShowingDto>>(
                    showingsFromRepo.Where(a => a.ProspectID == userId)));
            showingsToReturn.AddRange(
                _mapper.Map<IEnumerable<RealtorShowingDto>>(
                    showingsFromRepo.Where(a => a.RealtorID == userId)));
            showingsToReturn.AddRange(
                _mapper.Map<IEnumerable<UnformattedShowingDto>>(
                    showingsFromRepo.Where(a => a.RealtorID != userId &&
                                                a.ProspectID != userId)));

            return Ok(showingsToReturn);
        }
        /// <summary>
        /// Queries a single Showing from the database with the specified ID.
        /// </summary>
        /// <param name="showingId">The ID of the Showing to be returned. Provided in the request route.</param>
        /// <param name="userId">The ID of the user making the API calls. Provided in the header.</param>
        /// <returns>The record with the specified id as an OutboundShowingDto.</returns>
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
        /// <summary>
        /// Updates a Showing record in the database. Neglected fields are set to their default value (Null).
        /// </summary>
        /// <param name="showingId">The ID of the Showing to update. Provided in the Route.</param>
        /// <param name="showing">The UpdateShowingDto that holds the new field values. Provided in the body.</param>
        /// <param name="userId">The ID of the user making the api request. Provided in the Headers.</param>
        /// <returns>If the User or Showing are not found BadRequest is returned. If successful NoContent is returned.</returns>
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

            _showingService.Update(showingFromRepo);

            return NoContent();
        }
        /// <summary>
        /// Updates a Showing record in the database using a JSON patch document.
        /// </summary>
        /// <param name="showingId">The ID of the record to be updated. Provided in the route.</param>
        /// <param name="patchDocument">The JSON patch document. Provided in the body.</param>
        /// <param name="userId">The ID of the user making the api calls. Provided in the header.</param>
        /// <returns>BadRequest if the record or user is not found. No content if successful.</returns>
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
            _showingService.Update(showingToPatch);

            return NoContent();
        }
        /// <summary>
        /// Removes a specific Showing from the database.
        /// </summary>
        /// <param name="showingId">The ID of the Showing to be deleted. Provided in the routed.</param>
        /// <param name="userId">The Id of the User making the API call. Provided in the header.</param>
        /// <returns>BadRequest if the record or user is not found. No content if successful.</returns>
        [HttpDelete("{showingId:length(24)}")]
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
        /// <summary>
        /// Returns the methods supported at the resource endpoint.
        /// </summary>
        /// <returns>Returns an OK with the allowed request methods in the header.</returns>
        [HttpOptions]
        public IActionResult GetShowingsResourceOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,HEAD");
            return Ok();
        }
        /// <summary>
        /// Returns the methods supported at a unique record endpoint.
        /// </summary>
        /// <returns>Returns an OK with the allowed request methods in the header.</returns>
        [HttpOptions("{propertyId:length(24)}")]
        public IActionResult GetShowingsRecordOptions()
        {
            Response.Headers.Add("Allow", "GET,PUT,PATCH,DELETE");
            return Ok();
        }
    }
}
