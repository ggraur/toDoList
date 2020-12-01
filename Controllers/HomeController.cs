using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoList.Models;

namespace toDoList.Controllers
{
   

    public class HomeController : Controller
    {
       
        [Route("")]
        [Route("Home")]
        [Route("Home/Index")]
        public  ActionResult  Index()
        {
             return View("~/Views/Home/Index.cshtml");

            // return RedirectToAction("index", "User");
            //return View("~/Views/User/getUsers.cshtml");
        }
    }
}
