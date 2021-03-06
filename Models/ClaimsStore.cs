﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace toDoList.Models
{
    public static class ClaimsStore
    {
        public static List<Claim> AllClaims = new List<Claim>()
        {
            new Claim("Create User","Create User"),
            new Claim("Edit User","Edit User"),
            new Claim("Delete User","Delete User"),
            new Claim("Create Role","Create Role"),
            new Claim("Edit Role","Edit Role"),
            new Claim("Delete Role","Delete Role")
        };
    }
}
