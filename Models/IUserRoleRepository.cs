﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoClassLibrary;

namespace toDoList.Models
{
    public interface IUserRoleRepository
    {
        UserRole GetUserRole(int RoleID);
        public IEnumerable<UserRole> GetRoles();
    }
}
