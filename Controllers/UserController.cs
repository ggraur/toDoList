using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoList.Models;
using toDoList.ViewModels;
using toDoClassLibrary;

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
        [Route("~")]
        public ViewResult Index()
        {
            var model = _userRepository.GetUsers();
            return View("~/Views/User/GetUsers.cshtml", model);
        }
        
        [Route("Details/{id?}")]
        public ViewResult Details(int? Id)
        {
            UserDetailsViewModel userDetailsViewModel = new UserDetailsViewModel();
            userDetailsViewModel.User = _userRepository.GetUserDetails(Id ?? 1);
            userDetailsViewModel.PageTitle = "User Details";
            return View("~/Views/User/GetUserDetails.cshtml", userDetailsViewModel);
        }
        [HttpGet]
        public ViewResult Create()
        {
            return View("~/Views/User/Create.cshtml");
        }
        
        [HttpPost]
        public ViewResult Create(User _user)
        {
            if (ModelState.IsValid)
            {
                User newUser = _userRepository.Add(_user);
                UserDetailsViewModel userDetailsViewModel = new UserDetailsViewModel();
                userDetailsViewModel.User = newUser;
                userDetailsViewModel.PageTitle = "User Details";
                return View("~/Views/User/GetUserDetails.cshtml", userDetailsViewModel);
            }
            return View();
        }
    }
}
