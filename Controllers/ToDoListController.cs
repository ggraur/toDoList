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
        public ToDoListController(IToDoListRepository _toDoListRepository, ITaskRepository _taskRepository)
        {
            this.toDoListRepository = _toDoListRepository;
            this.taskRepository = _taskRepository;
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
        [HttpGet]
        [Route("Create")]
        public ViewResult Create()
        {
            return View("~/Views/ToDoList/Create.cshtml");
        }

        //// POST: TaskController/Create
        [HttpPost]
        [Route("Create")]
        public IActionResult Create(ToDoListCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                ToDoList _newList = new ToDoList
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
            //ToDoTask _newTask = _taskRepository.Details(id);

            //if (_newTask == null)
            //{
            //    Response.StatusCode = 404;
            //    return View("TaskNotFound", id);
            //}

            //TaskCreateViewModel taskCreateViewModel = new TaskCreateViewModel()
            //{
            //    TaskID = _newTask.TaskID,
            //    TaskName = _newTask.TaskName,
            //    TaskDescription = _newTask.TaskDescription,
            //    TaskActive = _newTask.TaskActive
            //};

            return View("~/Views/ToDoList/Edit.cshtml");//, taskCreateViewModel);
        }

        // POST: TaskController/Edit/5
        [HttpPost]
        [Route("Edit")]
        public ActionResult Edit(ToDoListCreateViewModel model)
        {
            try
            {
                //    ToDoTask _todoTask = new ToDoTask();
                //    _todoTask = _taskRepository.Details(model.TaskID);
                //    if (ModelState.IsValid && _todoTask != null)
                //    {
                //        _todoTask.TaskID = model.TaskID;
                //        _todoTask.TaskName = model.TaskName;
                //        _todoTask.TaskDescription = model.TaskDescription;
                //        _todoTask.TaskActive = model.TaskActive;

                //    }
                //    _taskRepository.Update(_todoTask);
                return RedirectToAction("Details");//, _todoTask.TaskID);
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
    }
}
