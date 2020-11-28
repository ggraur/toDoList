using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoList.Models;

namespace toDoList.Controllers
{
    public class UserRoleController : Controller
    {
        private IUserRoleRepository _userRoleRepository;

        public UserRoleController(IUserRoleRepository userRoleRepository)
        {
            _userRoleRepository =new MockUserRoleRepository();
        }
        public string Index()
        {
            return _userRoleRepository.GetUserRoleID(1).RoleDesc;
        }
    }
}
