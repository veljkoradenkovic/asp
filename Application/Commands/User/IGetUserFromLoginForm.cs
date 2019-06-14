﻿using Application.DataTransfer;
using Application.Interfaces;
using Application.SearchObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Commands.User
{
    public interface IGetUserFromLoginForm : ICommand<UserSearch, UserDto>
    {
    }
}