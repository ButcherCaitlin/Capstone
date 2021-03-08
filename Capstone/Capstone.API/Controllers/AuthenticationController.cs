using AutoMapper;
using Capstone.API.Authentication;
using Capstone.API.Entities;
using Capstone.API.Models;
using Capstone.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Capstone.API.Controllers
{
    [Route("api/authenticate")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationService _authService;
        private readonly DataService _dataService;
        private readonly IMapper _mapper;
        private readonly IOptions<JwtAuthentication> _jwtAuthentication;
        public AuthenticationController(AuthenticationService authService, DataService dataService,
            IMapper mapper, IOptions<JwtAuthentication> jwtAuthentication)
        {
            _dataService = dataService ??
                throw new ArgumentNullException(nameof(dataService));
            _authService = authService ??
                throw new ArgumentNullException(nameof(authService));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _jwtAuthentication = jwtAuthentication ??
                throw new ArgumentNullException(nameof(jwtAuthentication));
        }

        //get a specific user.
        [HttpGet("users")]
        [HttpHead("users")]
        public async Task<ActionResult<OutboundUserDto>> GetUser([FromQuery] string email)
        {
            if (email == null) return BadRequest();
            //get the user from repo using the email.
            var user = await _authService.GetUserAsync(email);

            //get the users a new token that will not be authenticated.
            user.AuthToken = _jwtAuthentication.Value.GenerateToken(user);

            //not sure how IOPTIONS works...
            return Ok(_mapper.Map<OutboundUserDto>(user));

            //maybe redefine and map the outbound user dto?
        }

        //add and login a user
        [HttpPost("users")]
        public async Task<ActionResult<OutboundUserDto>> CreateAndAuthenticateUser(CreateUserDto user)
        {
            //map the dto to a user entity 
            var userEntity = _mapper.Map<User>(user);
            //add the user to the repo
            var response = await _authService.AddUserAsync(userEntity);
            //if the user was added successfully authenticate them
            if (response.User != null) response.User.AuthToken = _jwtAuthentication.Value.GenerateToken(response.User);
            //if we could not authenticate send a bad request
            if (!response.Success) return BadRequest(new { error = response.ErrorMessage });
            //else return the mapped authenticated user
            return Ok(_mapper.Map<OutboundUserDto>(response.User));
        }

        //delete an existing user
        //[HttpDelete("users")]
        //public async Task<IActionResult> DeleteUser(PasswordObject content)
        //{
        //    var email = GetUserEmailFromToken(Request);
        //    if (email.StartsWith("Error")) return BadRequest(email);

        //    var user = await _authService.GetUserAsync(email);
        //    if (!HashPassword.Verify(content.Password, user.HashedPassword))
        //    {
        //        return BadRequest("Provided password does not match user password.");
        //    }

        //    return Ok(await _authService.DeleteUserAsync(email));
        //}

        //login an already existing user
        [HttpPost]
        public async Task<ActionResult<OutboundUserDto>> LogIn(CreateUserDto user)
        {
            //map to a user entity
            var userEntity = _mapper.Map<User>(user);
            //create a token for the user provided
            userEntity.AuthToken = _jwtAuthentication.Value.GenerateToken(userEntity);
            //attempt to authenticate the users token
            var result = await _authService.LoginUserAsync(userEntity);
            //if the user was logged in return the user with the authorized token if not return the failed result
            return result.User != null ?
                Ok(new UserResponses(result.User)) :
                Ok(result);
            //I did not use the DTO here. we should probably change it to something else
        }

        //logout an existing user
        //[HttpDelete]
        //public async Task<IActionResult> LogOut()
        //{
        //    var email = GetUserEmailFromToken(Request);
        //    if (email.StartsWith("Error")) return BadRequest(email);

        //    var result = await _authService.LogoutUserAsync(email);
        //    return Ok(result);
        //}
    }
}
