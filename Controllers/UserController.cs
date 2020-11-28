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
        public string Index()
        {
            return _userRepository.GetUser(1).UserName;
        }
        public ViewResult GetUsers()
        {
            var model = _userRepository.GetUsers();
            return View("~/Views/User/GetUsers.cshtml", model);

            //UserDetailsViewModel userDetailsViewModel = new UserDetailsViewModel();
            //userDetailsViewModel.GetUsers = _userRepository.GetUsers();
            //userDetailsViewModel.PageTitle = "All Users Registries";
            //return View("~/Views/User/GetUsers.cshtml", userDetailsViewModel);
        }

        public ViewResult UserDetails()
        {
            UserDetailsViewModel userDetailsViewModel = new UserDetailsViewModel();
            userDetailsViewModel.User = _userRepository.GetUser(1);
            userDetailsViewModel.PageTitle = "User Details";
            return View("~/Views/User/UserDetails.cshtml", userDetailsViewModel);
        }
    }
}
