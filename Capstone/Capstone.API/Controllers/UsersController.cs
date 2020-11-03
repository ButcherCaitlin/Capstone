﻿using AutoMapper;
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

namespace Capstone.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DatabaseService<User> _userService;
        private readonly IMapper _mapper;
        public UsersController(DatabaseService<User> userService,
            IMapper mapper)
        {
            _userService = userService ??
                throw new ArgumentNullException(nameof(userService));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
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
        /// <summary>
        /// Creates a new User record in the database.
        /// </summary>
        /// <param name="property">The CreateUserDto used to create the entity. Provided in the request
        /// body.</param>
        /// <returns>The newly created User from the databse as an OutboundUserDto, and its location.</returns>
        [HttpPost]
        public ActionResult<OutboundUserDto> Create(CreateUserDto user)
        {
            var createdUser = _userService.Create(_mapper.Map<User>(user));

            return CreatedAtRoute("GetUserById", new { userId = createdUser.Id.ToString() }, _mapper.Map<OutboundUserDto>(createdUser));
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
    }
}
