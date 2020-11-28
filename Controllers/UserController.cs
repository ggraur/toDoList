using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoList.Models;

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
        public ViewResult Details()
        {
            User model = _userRepository.GetUser(1);
            ViewData["User"] = model;
            ViewData["PageTitle"] = "User Details";

            return View("Details");
        }
    }
}
