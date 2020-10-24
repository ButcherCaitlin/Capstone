using AutoMapper;
using Capstone.API.Entities;
using Capstone.API.Models;
using Capstone.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Capstone.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IMapper _mapper;

        public UsersController(UserService userService,
            IMapper mapper)
        {
            _userService = userService ??
                throw new ArgumentNullException(nameof(userService));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }
        //This will get all users
        [HttpGet]
        [HttpHead]
        public ActionResult<IEnumerable<UserDto>> GetUsers()
        {
            var usersFromRepo = _userService.Get();
            return Ok(_mapper.Map<IEnumerable<UserDto>>(usersFromRepo));
        }
        //this will get a specific user
        [HttpGet("{userId:length(24)}", Name = "GetUser")]
        public ActionResult<UserDto> GetUser(string userId)
        {
            var user = _userService.Get(userId);
            if (user == null) return NotFound();
            return Ok(_mapper.Map<UserDto>(user));
        }
        //this will create a single user
        [HttpPost]
        public ActionResult<User> Create(UserDto user)
        {
            var createdUser = _userService.Create(_mapper.Map<User>(user));
            return CreatedAtRoute("GetUser", new { userId = createdUser.Id.ToString() }, createdUser);
        }
    }
}
