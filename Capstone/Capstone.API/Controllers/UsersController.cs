using AutoMapper;
using Capstone.API.Authentication;
using Capstone.API.Entities;
using Capstone.API.Models;
using Capstone.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Capstone.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IMapper _mapper;
        private readonly IOptions<JwtAuthentication> _jwtAuthentication;
        public UsersController(UserService userService,
            IMapper mapper, IOptions<JwtAuthentication> jwtAuthentication)
        {
            _userService = userService ??
                throw new ArgumentNullException(nameof(userService));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _jwtAuthentication = jwtAuthentication ??
                throw new ArgumentNullException(nameof(jwtAuthentication));
        }
        /// <summary>
        /// Queries all User records from the database.
        /// </summary>
        /// <returns>All records in the database are returned as OutboundPropertyDto's.</returns>
        [HttpGet]
        [HttpHead]
        public ActionResult<IEnumerable<OutboundUserDto>> GetUsers()
        {
            var usersFromRepo = _userService.Get();

            return Ok(_mapper.Map<IEnumerable<OutboundUserDto>>(usersFromRepo));
        }
        /// <summary>
        /// Queries a single User from the database with the specified ID.
        /// </summary>
        /// <param name="userId">The ID of the User to be returned. Provided in the request route.
        /// </param>
        /// <returns>The record with the specified id as an OutboundUserDto.</returns>
        [HttpGet("{userId:length(24)}", Name = "GetUserById")]
        [HttpHead("{userId:length(24)}")]
        public ActionResult<OutboundUserDto> GetUser(string userId)
        {
            var user = _userService.Get(userId);
            if (user == null) return NotFound();

            return Ok(_mapper.Map<OutboundUserDto>(user));
        }

        [HttpGet]
        [HttpHead]
        public async Task<ActionResult<OutboundUserDto>> GetUserByEmail([FromQuery] string email)
        {
            var user = await _userService.GetUserAsync(email);
            if (user == null) return NotFound();
            user.AuthToken = _jwtAuthentication.Value.GenerateToken(user);
            var outboundUser = _mapper.Map<OutboundUserDto>(user);
            return Ok(outboundUser);
        }

        /// <summary>
        /// Creates a new User record in the database.
        /// </summary>
        /// <param name="property">The CreateUserDto used to create the entity. Provided in the request
        /// body.</param>
        /// <returns>The newly created User from the databse as an OutboundUserDto, and its location.</returns>
        [HttpPost]
        public async Task<ActionResult<OutboundUserDto>> Create(CreateUserDto user)
        {
            var createUserResponse = await _userService.AddUserAsync(_mapper.Map<User>(user));
            if (createUserResponse.User != null) createUserResponse.User.AuthToken = _jwtAuthentication.Value.GenerateToken(createUserResponse.User);


            if (!createUserResponse.Success)
            {
                return BadRequest(new { error = createUserResponse.ErrorMessage });
            }
            return CreatedAtRoute("GetUserById", new { userId = createUserResponse.User.Id.ToString() },
                _mapper.Map<OutboundUserDto>(createUserResponse.User));
        }

        /// <summary>
        /// Upserts a User record in the database. If updating, neglected fields are set to their default value (Null).
        /// </summary>
        /// <param name="userId">The ID of the User object. Provided in the Headers.</param>
        /// <param name="user">The UpdateUserDto that holds the new field values. Provided in the body.</param>
        /// <returns>If it is created, the newly created User from the databse as an OutboundUserDto is returned,
        /// as well as its location.</returns>
        [HttpPut("{userId:length(24)}")]
        public IActionResult UpdateUser(string userId, UpdateUserDto user)
        {
            var userFromRepo = _userService.Get(userId);
            if (userFromRepo == null)
            {
                var userToAdd = _mapper.Map<User>(user);
                userToAdd.Id = userId;

                _userService.Create(userToAdd);

                return CreatedAtRoute("GetUserById",
                    new { userId },
                    _mapper.Map<OutboundUserDto>(userToAdd));
            }
            _mapper.Map(user, userFromRepo);

            _userService.Update(userFromRepo);

            return NoContent();
        }
        /// <summary>
        /// Updates a User record in the database using a JSON patch document.
        /// </summary>
        /// <param name="userId">The ID of the record to be updated. Provided in the route.</param>
        /// <param name="patchDocument">The JSON patch document. Provided in the body.</param>
        /// <returns>BadRequest if the record is not found. No content if successful.</returns>
        [HttpPatch("{userId:length(24)}")]
        public IActionResult PartiallyUpdateUser(string userId,
            JsonPatchDocument<UpdateUserDto> patchDocument)
        {
            var userToPatch = _userService.Get(userId);
            if (userToPatch == null) return NotFound(new { message = "If you are trying to upsert a resource use PUT" });

            var userDtoToPatch = _mapper.Map<UpdateUserDto>(userToPatch);
            patchDocument.ApplyTo(userDtoToPatch, ModelState);
            if (!TryValidateModel(userDtoToPatch)) return ValidationProblem(ModelState);

            _mapper.Map(userDtoToPatch, userToPatch);
            _userService.Update(userToPatch);

            return NoContent();
        }
        /// <summary>
        /// Removes a specific User from the database.
        /// </summary>
        /// <param name="userId">The ID of the User to be deleted. Provided in the routed.</param>
        /// <returns>BadRequest if the user is not found. No content if successful.</returns>
        [HttpDelete("{userId:length(24)}")]
        public IActionResult DeleteUser(string userId)
        {
            var userToDelete = _userService.Get(userId);
            if (userToDelete == null) return NotFound();

            _userService.Remove(userToDelete.Id);

            return NoContent();


            /////[HttpDelete("api/user/delete")]
            /////[Authorize(AuthenticationSchemes = "Bearer")]

            //public async Task<ActionResult> Delete([FromBody] PasswordObject content)
            //{
            //    var email = GetUserEmailFromToken(Request);
            //    if (email.StartsWith("Error")) return BadRequest(email);

            //    var user = await userRepository.GetUserAsync(email);
            //    if (!HashPassword.Verify(content.Password, user.HashedPassword))
            //    {
            //        return BadRequest("Provided password does not match user password.");
            //    }

            //    return Ok(await userRepository.DeleteUserAsync(email));
            //}
        }
        /// <summary>
        /// Returns the methods supported at the resource endpoint.
        /// </summary>
        /// <returns>Returns an OK with the allowed request methods in the header.</returns>
        [HttpOptions]
        public IActionResult GetUsersResourceOptions()
        {
            Response.Headers.Add("Allow", "GET,POST,OPTIONS,HEAD");
            return Ok();
        }
        /// <summary>
        /// Returns the methods supported at a unique record endpoint.
        /// </summary>
        /// <returns>Returns an OK with the allowed request methods in the header.</returns>
        [HttpOptions("{userId:length(24)}")]
        public IActionResult GetUsersRecordOptions()
        {
            Response.Headers.Add("Allow", "GET,PUT,PATCH,DELETE");
            return Ok();
        }
        /// <summary>
        /// Creates and returns an Invalid model state response to the consumer.
        /// </summary>
        /// <param name="modelStateDictionary">The current ModelState.</param>
        /// <returns>An invalid IActionresult object.</returns>
        public override ActionResult ValidationProblem(
            [ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }

        /////[HttpPost("api/user/login")]
        //public async Task<ActionResult> Login([FromBody] Users user)
        //{
        //    user.AuthToken = jwtAuthentication.Value.GenerateToken(user);
        //    var result = await userRepository.LoginUserAsync(user);
        //    return result.Users != null ? Ok(new UserResponses(result.Users)) : Ok(result);
        //}

        /////[HttpPost("api/user/logout")]
        /////[Authorize(AuthenticanSchemes = "Bearer")]
        //public async Task<ActionResult> Logout()
        //{
        //    var email = GetUserEmailFromToken(Request);
        //    if (email.StartsWith("Error")) return BadRequest(email);

        //    var result = await userRepository.LogoutUserAsync(email);
        //    return Ok(result);
        //}
    }
}
