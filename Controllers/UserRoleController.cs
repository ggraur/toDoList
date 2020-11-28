using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoList.Models;
using toDoList.ViewModels;

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
            return _userRoleRepository.GetUserRole(1).RoleDesc;
        }
        public ViewResult RoleDetails()
        {
            UserRoleViewModel userRoleViewModel = new UserRoleViewModel();

            userRoleViewModel.UserRole= _userRoleRepository.GetUserRole(1);
            userRoleViewModel.PageTitle = "User Role Details";
            return View("~/Views/UserRole/RoleDetails.cshtml", userRoleViewModel);
        }
    }
}
