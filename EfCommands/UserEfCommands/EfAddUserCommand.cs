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
    public class EfAddUserCommand : BaseCommand, IAddUserCommand
    {
        public EfAddUserCommand(NewsContext _context) : base(_context)
        {
        }

        public void Execute(CreateUserDto request)
        {
            if (request == null)
            {
                throw new NullReferenceException();
            }
            else
            {
                if(Context.Users.Any(u => u.Username == request.Username))
                {
                    throw new EntityAlreadyExistsException("User");
                }
                try
                {
                    Context.Users.Add(new Domain.User
                    {
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Password = HashPasswordCommand.MD5Hash(request.Password),
                        Username = request.Username,
                        Email = request.Email,
                        RoleId = 2,
                        CreatedAt = DateTime.Now
                    });
                    Context.SaveChanges();
                    
                }
                catch
                {
                    throw new NullReferenceException();
                }
            }
        }
    }
}
