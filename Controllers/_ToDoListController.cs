using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoClassLibrary;
using toDoList.Models;
using toDoList.ViewModels;

namespace toDoList.Controllers
{
    [Route("controller")]
    public class ToDoListController : Controller
    {
        private readonly IToDoListRepository toDoListRepository;
        private readonly ITaskRepository taskRepository;
        private readonly IUserRepository userRepository;

        public ToDoListController(IToDoListRepository _toDoListRepository, ITaskRepository _taskRepository, IUserRepository _userRepository)
        {
            this.toDoListRepository = _toDoListRepository;
            this.taskRepository = _taskRepository;
            this.userRepository = _userRepository;
        }

        [HttpGet]
        public ViewResult Index()
        {
            var model = toDoListRepository.Details();
            return View("~/Views/ToDoList/Index.cshtml", model);
        }

        [Route("Details/{id?}")]
        public ViewResult Details(int? Id)
        {
            ToDoListDetailsViewModel todoList = new ToDoListDetailsViewModel();

            ToDoList _tmpToDo = toDoListRepository.Details((int)Id);
            if (_tmpToDo == null)
            {
                Response.StatusCode = 404;
                return View("NotFound", Id);
            }
            todoList.Tasks = taskRepository.ActiveTasks();
            todoList.ToDoList = _tmpToDo;
            todoList.PageTitle = "ToDo List Details";
            return View("~/Views/ToDoList/Details.cshtml", todoList);
        }
        //[HttpGet]
        //[Route("Create")]
        //public ViewResult Create()
        //{
        //    return View("~/Views/ToDoList/Create.cshtml");
        //}

        [HttpGet]
        [Route("Create/{id?}")]
        public ViewResult Create(int id)
        {
            var _user = userRepository.GetUserDetails(id);
            ToDoListCreateViewModel model = new ToDoListCreateViewModel();
            model.UserIDExecutor = _user.UserName;
            return View("~/Views/ToDoList/Create.cshtml", model);
        }
        //// POST: TaskController/Create
        [HttpPost]
        [Route("Create")]
        public IActionResult Create(ToDoList model)
        {
            if (ModelState.IsValid)
            {
                ToDoListCreateViewModel _newList = new ToDoListCreateViewModel
                {
                    ToDoListName = model.ToDoListName,
                    CreatedToDoListDatetime = model.CreatedToDoListDatetime,
                    FinalizationDatetime = model.FinalizationDatetime,
                    //ToDoTasks = new List<ToDoTask>(),
                    UserIDCreator = model.UserIDCreator,
                    UserIDExecutor = model.UserIDExecutor
                };
                toDoListRepository.Add(_newList);
                return RedirectToAction("Details", new { id = _newList.ToDoListID });
                //return View("~/Views/ToDoList/_Layout.cshtml", new { id = _newList.ToDoListID });
            }
            return View();
        }

        // GET: TaskController/Edit/5
        [HttpGet]
        [Route("Edit")]
        public ActionResult Edit(int id)
        {
            ToDoList _newTask = toDoListRepository.Details(id);

            if (_newTask == null)
            {
                Response.StatusCode = 404;
                return View("TaskNotFound", id);
            }

            ToDoList _todoList = new ToDoList()
            {
                ToDoListID = _newTask.ToDoListID,
                ToDoListName = _newTask.ToDoListName,
                CreatedToDoListDatetime = _newTask.CreatedToDoListDatetime,
                FinalizationDatetime = _newTask.FinalizationDatetime,
                UserIDCreator = _newTask.UserIDCreator,
                UserIDExecutor = _newTask.UserIDExecutor
            };

            return View("~/Views/ToDoList/Edit.cshtml", _todoList);
        }

        // POST: TaskController/Edit/5
        [HttpPost]
        [Route("Edit")]
        public ActionResult Edit(ToDoList model)
        {
            try
            {
                ToDoList _todoList = new ToDoList();
                _todoList = toDoListRepository.Details(model.ToDoListID);
                if (ModelState.IsValid && _todoList != null)
                {
                    _todoList.ToDoListID = model.ToDoListID;
                    _todoList.ToDoListName = model.ToDoListName;
                    _todoList.CreatedToDoListDatetime = model.CreatedToDoListDatetime;
                    _todoList.FinalizationDatetime = model.FinalizationDatetime;
                    _todoList.UserIDCreator = model.UserIDCreator;
                    _todoList.UserIDExecutor = model.UserIDExecutor;

                }
                toDoListRepository.Update(_todoList);
                return RedirectToAction("index");
            }
            catch
            {
                return View();
            }
        }

        // GET: TaskController/Delete/5
        [Route("Delete")]
        public ActionResult Delete(int id)
        {
            try
            {
                ToDoList _newTask = new ToDoList();
                _newTask = toDoListRepository.Details(id);
                return View("~/Views/ToDoList/Delete.cshtml", _newTask);
            }
            catch
            {
                return View();
            }
        }

        // POST: TaskController/Delete/5
        [HttpPost]
        [Route("Delete")]
        public ActionResult Delete(ToDoList model)
        {
            try
            {
                ToDoList _newTask = new ToDoList();
                _newTask = toDoListRepository.Details(model.ToDoListID);
                toDoListRepository.Delete(_newTask);
                return RedirectToAction("index");
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult SaveListToDo(ToDoListCreateViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    toDoListRepository.Add(model);
                    return RedirectToAction("index");
                }
                return View("index");
            }
            catch
            {
                return View("index");
            }

        }
    }
}
