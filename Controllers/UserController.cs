using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoList.Models;
using toDoList.ViewModels;
using toDoClassLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace toDoList.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        [Obsolete]
        private readonly IHostingEnvironment _hostingEnvironment;

        [Obsolete]
        public UserController(IUserRepository userRepository, IHostingEnvironment hostingEnvironment)
        {
            //  _userRepository = new MockUserRepository();
            _userRepository = userRepository;
            _hostingEnvironment = hostingEnvironment;
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

            User _tmpUser = _userRepository.GetUserDetails((int)Id);
            if (_tmpUser == null)
            {
                Response.StatusCode = 404;
                return View("UserNotFound", Id);
            }

            userDetailsViewModel.User = _tmpUser;
            userDetailsViewModel.PageTitle = "User Details";
            return View("~/Views/User/GetUserDetails.cshtml", userDetailsViewModel);
        }
        [HttpGet]
        public ViewResult Create()
        {
            return View("~/Views/User/Create.cshtml");
        }

        //[HttpPost]
        //public ViewResult Create(User _user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        User newUser = _userRepository.Add(_user);
        //        UserDetailsViewModel userDetailsViewModel = new UserDetailsViewModel();
        //        userDetailsViewModel.User = newUser;
        //        userDetailsViewModel.PageTitle = "User Details";
        //        return View("~/Views/User/GetUserDetails.cshtml", userDetailsViewModel);
        //    }
        //    return View();
        //}

        [HttpPost]
        [Obsolete]
        public IActionResult Create(UserCreateViewModel model)

        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(model);
                User _newUser = new User
                {
                    UserName = model.UserName,
                    UserPass = model.UserPass,
                    UserRole = model.UserRole,
                    UserEmail = model.UserEmail,
                    PhotoPath = uniqueFileName
                };
                _userRepository.Add(_newUser);
                return RedirectToAction("Details", new { id = _newUser.UserID });

            }
            return View();
        }

        [HttpPost]
        [Obsolete]
        [Route("Edit")]
        public IActionResult Edit(UserEditViewModel model)
        {
            User user = _userRepository.GetUserDetails(model.UserID);
            bool modelHasNoPhotoBefore = false;
            if (model.ExistingPhotoPath == null && model.Photo.FileName != null)
            {
                user.PhotoPath = ProcessUploadedFile(model);
                model.ExistingPhotoPath = user.PhotoPath;
                modelHasNoPhotoBefore = true;
            }

            if (ModelState.IsValid)
            {
                user.UserName = model.UserName;
                user.UserPass = model.UserPass;
                user.UserRole = model.UserRole;
                user.UserEmail = model.UserEmail;
                if (model.Photo != null)
                {
                    if (modelHasNoPhotoBefore == false)
                    {
                        if (model.ExistingPhotoPath != null)
                        {
                            string filepath = Path.Combine(_hostingEnvironment.WebRootPath, "images", model.ExistingPhotoPath);
                            System.IO.File.Delete(filepath);
                        }
                        user.PhotoPath = ProcessUploadedFile(model);
                    }
                }
                _userRepository.Update(user);
                return RedirectToAction("Details", new { id = user.UserID });

            }
            return View();
        }

        [Obsolete]
        private string ProcessUploadedFile(UserCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
            }

            return uniqueFileName;
        }

        [HttpGet]
        [Route("Edit")]
        public ViewResult Edit(int id)
        {
            User user = _userRepository.GetUserDetails(id);
            if (user == null)
            {
                Response.StatusCode = 404;
                return View("UserNotFound", id);
            }
            UserEditViewModel userEdit = new UserEditViewModel()
                {
                    UserID = user.UserID,
                    UserName = user.UserName,
                    UserPass = user.UserPass,
                    UserEmail = user.UserEmail,
                    UserRole = user.UserRole,
                    ExistingPhotoPath = user.PhotoPath
                };
                return View("~/Views/User/Edit.cshtml", userEdit);
           
           

        }
    }
}
