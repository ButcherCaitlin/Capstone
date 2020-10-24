using AutoMapper;
using Capstone.API.Entities;
using Capstone.API.Models;
using Capstone.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Capstone.API.Controllers
{
    [Route("api/users/{userId}/showings")]
    [ApiController]
    public class ShowingsController : ControllerBase
    {
        private readonly ShowingService _showingService;
        private readonly UserService _userService;
        private readonly IMapper _mapper;

        public ShowingsController(ShowingService showingService,
            UserService userService,
            IMapper mapper)
        {
            _showingService = showingService ??
                throw new ArgumentNullException(nameof(showingService));
            _userService = userService ??
                throw new ArgumentNullException(nameof(userService));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }
        [HttpGet]
        public ActionResult<IEnumerable<ShowingDto>> GetShowingsForUser(string userId)
        {
            var userFromRepo = _userService.Get(userId);
            if (userFromRepo == null) return NotFound();
            var showingsFromRepo = _showingService.GetShowingsForUser(userId);

            var showingsToReturn = new List<ShowingDto>();
            foreach(var showing in showingsFromRepo)
            {
                if (showing.RealtorID == userId)
                {
                    showingsToReturn.Add(_mapper.Map<ShowingForRealtorDto>(showing));
                } else if (showing.RealtorID != userId)
                {
                    showingsToReturn.Add(_mapper.Map<ShowingForProspectDto>(showing));
                }
            }

            return Ok(showingsToReturn);
        }

        [HttpGet("{showingId:length(24)}", Name = "GetShowingForUser")]
        public ActionResult<ShowingDto> GetShowingForUser(string userId, string showingId)
        {
            var userFromRepo = _userService.Get(userId);
            if (userFromRepo == null) return NotFound();
            var showingFromRepo = _showingService.Get(userId, showingId);
            if (showingFromRepo == null) return NotFound();

            if (showingFromRepo.RealtorID == userId)
            {
                return Ok(_mapper.Map<ShowingForRealtorDto>(showingFromRepo));
            }
            else if (showingFromRepo.RealtorID != userId)
            {
                return Ok(_mapper.Map<ShowingForProspectDto>(showingFromRepo));
            }

            return NotFound();
        }
        [HttpPost]
        public ActionResult<ShowingDto> CreateShowing(string userId, ShowingDto showingDto)
        {
            _showingService.Create(_mapper.Map<Showing>(showingDto));
            return CreatedAtRoute("GetShowingForUser", new { showingId = showingDto.Id }, showingDto);
        }
    }
}
