using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Commands.User;
using Application.DataTransfer;
using Application.DataTransfer.Create;
using Application.Exceptions;
using Application.SearchObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IGetUsersCommand _getUsersCommand;
        private IGetUserCommand _getUserCommand;
        private IAddUserCommand _addUserCommand;
        private IEditUserCommand _editUserCommand;
        private IDeleteUserCommand _deleteUsersCommand;

        public UserController(IGetUsersCommand getUsersCommand, IAddUserCommand addUserCommand, IGetUserCommand getUserCommand, IDeleteUserCommand deleteUsersCommand, IEditUserCommand editUserCommand)
        {
            _getUsersCommand = getUsersCommand;
            _addUserCommand = addUserCommand;
            _getUserCommand = getUserCommand;
            _deleteUsersCommand = deleteUsersCommand;
            _editUserCommand = editUserCommand;
        }

        /// <summary>
        /// Returns a group of Users matching the given keyword.
        /// </summary>
        /// <param name="search">The username to search for</param>
        // GET: api/Gorivo
        // GET: api/User
        [HttpGet]
        public IActionResult Get([FromQuery] UserSearch search)
        {
            try
            {
                var dto = _getUsersCommand.Execute(search);
                return Ok(dto);
            }
            catch (EntityNotFoundException)
            {
                return Conflict("There's no data for your request.");
            }
        }

        /// <summary>
        /// Retrieve the user by their ID.
        /// </summary>
        /// <param name="id">The ID of the desired USer</param>
        /// <returns>A UserDto</returns>
        // GET: api/User/5
        [HttpGet("{id}", Name = "GetUser")]
        public ActionResult<UserDto> Get(int id)
        {
            try
            {
                var user = _getUserCommand.Execute(id);
                return Ok(user);

            }
            catch (EntityNotFoundException e)
            {
                if (e.Message == "User doesn't exist.")
                {
                    return NotFound(e.Message);
                }
                return UnprocessableEntity(e.Message);
            }
        }

        /// <summary>
        /// Kreiranje novog usera 
        /// </summary>
        /// <param name="user">DTO potreban za kreiranje novog usera</param>
        /// <returns>Status</returns>
        // POST: api/User
        [HttpPost]
        public IActionResult Post([FromBody] CreateUserDto user)
        {
            try
            {
                _addUserCommand.Execute(user);
                return Ok();
            }
            catch (EntityAlreadyExistsException e)
            {
                return UnprocessableEntity(e.Message);
            }
        }

        /// <summary>
        /// Izmena postojeceg korisnika
        /// </summary>
        /// <param name="user">DTO potreban za izmenu postojeceg korisnika</param>
        /// <returns>Status</returns>
        // PUT: api/User/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] CreateUserDto user)
        {
            try
            {
                user.UserId = id;
                _editUserCommand.Execute(user);
                return Ok();
            }
            catch(EntityAlreadyExistsException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return UnprocessableEntity(e.Message);
            }
        }

        /// <summary>
        /// Brisanje postojeceg korisnika
        /// </summary>
        /// <param name="id">id koji odgovara korisniku</param>
        /// <returns>Status</returns>
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _deleteUsersCommand.Execute(id);
                return NoContent();
            }
            catch (EntityNotFoundException)
            {
                return Conflict("That news is already deleted.");
            }
        }
    }
}
