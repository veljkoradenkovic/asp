using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.DataTransfer.Create
{
    public class CreateUserDto 
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [StringLength(30,ErrorMessage = "First name should have max 30 characters.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [StringLength(30, ErrorMessage = "Last name should have max 30 characters.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [StringLength(15, ErrorMessage = "Username should be unique and have max 15 characters.")]
        public string Username { get; set; }
        public int RoleId { get; set; }
    }
}
