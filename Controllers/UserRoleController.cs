using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoList.Models;
using toDoList.ViewModels;

namespace toDoList.Controllers
{
    [Route("[controller]")]
    public class UserRoleController : Controller
    {
        private IUserRoleRepository _userRoleRepository;

        public UserRoleController(IUserRoleRepository userRoleRepository)
        {
            _userRoleRepository =new MockUserRoleRepository();
        }
        
        [Route("Index")]
        [Route("~")]
        public ViewResult Index()
        {
            //return _userRoleRepository.GetUserRole(1).RoleDesc;
            var model = _userRoleRepository.GetRoles();
            return View("~/Views/UserRole/Roles.cshtml", model);
        }

        [Route("Details/{id?}")]
        public ViewResult RoleDetails(int? Id)
        {
            UserRoleViewModel userRoleViewModel = new UserRoleViewModel();

            userRoleViewModel.UserRole= _userRoleRepository.GetUserRole(Id ?? 1);
            userRoleViewModel.PageTitle = "User Role Details";
            return View("~/Views/UserRole/RoleDetails.cshtml", userRoleViewModel);
        }
    }
}
