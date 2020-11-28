using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoList.Models;
using toDoList.ViewModels;

namespace toDoList.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = new MockUserRepository();
        }
 
        public ViewResult GetUsers()
        {
            var model = _userRepository.GetUsers();
            return View("~/Views/User/GetUsers.cshtml", model);
   
        }

        //public ViewResult UserDetails(int Id)
        public ViewResult GetUserDetails(int Id)
        {
            UserDetailsViewModel userDetailsViewModel = new UserDetailsViewModel();
            userDetailsViewModel.User = _userRepository.GetUserDetails(Id);
            userDetailsViewModel.PageTitle = "User Details";
            return View("~/Views/User/GetUserDetails.cshtml", userDetailsViewModel);
        }
    }
}
