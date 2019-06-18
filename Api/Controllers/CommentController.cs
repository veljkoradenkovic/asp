using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Helpers;
using Application.Commands.Comment;
using Application.DataTransfer;
using Application.DataTransfer.Create;
using Application.Exceptions;
using Application.SearchObjects;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{    [Route("api/[controller]")]
    public class CommentController : Controller
    {
        private IGetCommentCommand _getCommentCommand;
        private IGetCommentsCommand _getCommentsCommand;
        private IAddCommentCommand _addCommentCommand;
        private IEditCommentCommand _editCommentCommand;
        private IDeleteCommentCommand _deleteCommentCommand;

        public CommentController(IGetCommentCommand getCommentCommand, IGetCommentsCommand getCommentsCommand, IAddCommentCommand addCommentCommand, IEditCommentCommand editCommentCommand, IDeleteCommentCommand deleteCommentCommand)
        {
            _getCommentCommand = getCommentCommand;
            _getCommentsCommand = getCommentsCommand;
            _addCommentCommand = addCommentCommand;
            _editCommentCommand = editCommentCommand;
            _deleteCommentCommand = deleteCommentCommand;
        }

        /// <summary>
        /// Returns a group of Comments matching the given keyword.
        /// </summary>
        /// <param name="search">The comment to search for</param>
        // GET: api/Gorivo
        // GET: api/Comment
        [HttpGet]
        public IActionResult Get([FromQuery] CommentSearch search)
        {
            try
            {
                var dto = _getCommentsCommand.Execute(search);
                return Ok(dto);
            }
            catch (EntityNotFoundException)
            {
                return Conflict("There's no data for your request.");
            }
        }

        /// <summary>
        /// Retrieve the comment by their ID.
        /// </summary>
        /// <param name="id">The ID of the desired Comment</param>
        /// <returns>A CommentDto</returns>
        // GET api/Comment/5
        [HttpGet("{id}")]
        public ActionResult<CommentDto> Get(int id)
        {
            try
            {
                var dto = _getCommentCommand.Execute(id);
                return Ok(dto);
            }
            catch (EntityNotFoundException e)
            {
                if (e.Message == "Comment doesn't exist.")
                {
                    return NotFound(e.Message);
                }
                return UnprocessableEntity(e.Message);
            }
        }

        /// <summary>
        /// Kreiranje novog komentara
        /// </summary>
        /// <param name="createDto">DTO potreban za kreiranje novog komentara</param>
        /// <returns>Status</returns>
        // POST api/Comment
        [LoggedIn("Admin")]
        [HttpPost]
        public IActionResult Post([FromBody]CreateCommentDto createDto)
        {
            try
            {
                _addCommentCommand.Execute(createDto);
                return StatusCode(201);

            }
            catch
            {
                return StatusCode(500, "An error has occured.");
            }
        }

        /// <summary>
        /// Izmena postojeceg komentara
        /// </summary>
        /// <param name="createDto">DTO potreban za izmenu postojeceg komentara</param>
        /// <returns>Status</returns>
        // PUT api/Comment/5
        [LoggedIn("Admin")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]CreateCommentDto createDto)
        {
            createDto.CommentId = id;
            try
            {

                _editCommentCommand.Execute(createDto);
                return NoContent();
            }
            catch (EntityNotFoundException e)
            {
                if (e.Message == "Comment doesn't exist.")
                {
                    return NotFound(e.Message);
                }

                return UnprocessableEntity(e.Message);

            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Brisanje postojeceg komentara
        /// </summary>
        /// <param name="id">id koji odgovara komentaru</param>
        /// <returns>Status</returns>
        // DELETE api/Comment/5
        [LoggedIn("Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _deleteCommentCommand.Execute(id);
                return NoContent();
            }
            catch (EntityNotFoundException e)
            {
                if (e.Message == "Comment doesn't exist.")
                {
                    return NotFound(e.Message);
                }

                return UnprocessableEntity(e.Message);
            }
        }
    }
}
