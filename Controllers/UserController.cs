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
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = new MockUserRepository();
        }
        [Route("")]
        [Route("Index")]
        [Route("~")]
        [HttpGet]
        public ViewResult GetUsers()
        {
            var model = _userRepository.GetUsers();
            return View("~/Views/User/GetUsers.cshtml", model);
        }

        [Route("Details/{id?}")]
        [HttpGet]
        public ViewResult GetUserDetails(int? Id)
        {
            UserDetailsViewModel userDetailsViewModel = new UserDetailsViewModel();
            userDetailsViewModel.User = _userRepository.GetUserDetails(Id ?? 1);
            userDetailsViewModel.PageTitle = "User Details";
            return View("~/Views/User/GetUserDetails.cshtml", userDetailsViewModel);
        }
        [Route("create")]
        [HttpGet]
        public ViewResult Create()
        {
            return View("~/Views/User/Create.cshtml");
        }
        [Route("create")]
        [HttpPost]
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                User newUser = _userRepository.Add(user);

                UserDetailsViewModel userDetailsViewModel = new UserDetailsViewModel();
                userDetailsViewModel.User = _userRepository.GetUserDetails(newUser.UserID);
                userDetailsViewModel.PageTitle = "User Details";
                return RedirectToAction("Details", "User", "../");
                //return RedirectToAction("Details", userDetailsViewModel);
            }
            return View();
        }
    }
}
