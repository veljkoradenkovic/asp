using Application.Commands.User;
using Application.DataTransfer.Create;
using Application.Exceptions;
using EfDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EfCommands.UserEfCommands
{
    public class EfEditUserCommand : BaseCommand, IEditUserCommand
    {
        public EfEditUserCommand(NewsContext _context) : base(_context)
        {
        }

        public void Execute(CreateUserDto request)
        {
            var user = Context.Users.Find(request.UserId);
            if (user == null)
            {
                throw new EntityNotFoundException("User");
            }
            if (Context.Users.Where(x => x.Id != request.UserId).Any(x => x.Username == request.Username))
            {
                throw new EntityAlreadyExistsException("User");
            }
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.Username = request.Username;
            user.RoleId = request.RoleId;
            user.Password = HashPasswordCommand.MD5Hash(request.Password);
            user.ModifiedAt = DateTime.Now;
            Context.SaveChanges();
        }
    }
}
