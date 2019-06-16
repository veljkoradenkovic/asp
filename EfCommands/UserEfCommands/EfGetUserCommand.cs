using Application.Commands.User;
using Application.DataTransfer;
using Application.Exceptions;
using EfDataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EfCommands.UserEfCommands
{
    public class EfGetUserCommand : BaseCommand, IGetUserCommand
    {
        public EfGetUserCommand(NewsContext _context) : base(_context)
        {
        }

        public UserDto Execute(int request)
        {
            var users = Context.Users.Include(x => x.Role).AsQueryable();
            var user = users.Where(x => x.Id == request).FirstOrDefault();
            if (user == null)
                throw new EntityNotFoundException("User");

            return new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.Username,
                Id = user.Id,
                RoleName = user.Role.Name
            };
        }
    }
}
