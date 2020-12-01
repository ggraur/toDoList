using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoList.Models;
using toDoClassLibrary;
using toDoList.ViewModels;

namespace toDoList.Controllers
{
    [Route("[controller]")]
    public class TaskController : Controller
    {
        private readonly ITaskRepository _taskRepository;

        public TaskController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        [HttpGet]
        // GET: TaskController
        public ActionResult Index()
        {
            var model = _taskRepository.Tasks();
            return View("~/Views/Task/Index.cshtml", model);
        }

        // GET: TaskController/Details/5
        [HttpGet]
        [Route("Details/{id?}")]
        public ViewResult Details(int? Id)
        {
            TaskDetailsViewModel taskDetailsViewModel = new TaskDetailsViewModel();

            ToDoTask _tmpTask = _taskRepository.Details((int)Id);
            if (_tmpTask == null)
            {
                Response.StatusCode = 404;
                return View("TaskNotFound", Id);
            }

            taskDetailsViewModel.ToDoTask = _tmpTask;
            taskDetailsViewModel.PageTitle = "User Details";
            return View("~/Views/Task/TaskDetails.cshtml", taskDetailsViewModel);
        }

        // GET: TaskController/Create
        [HttpGet]
        [Route("Create")]
        public ViewResult Create()
        {
            return View("~/Views/Task/Create.cshtml");
        }

        // POST: TaskController/Create
        [HttpPost]
        [Route("Create")]
        public IActionResult Create(TaskCreateViewModel model)
        {
            if (ModelState.IsValid)
            {

                ToDoTask _newTask = new ToDoTask
                {
                    TaskName = model.TaskName,
                    TaskDescription = model.TaskDescription,
                    TaskActive=model.TaskActive

                };
                _taskRepository.Add(_newTask);
                return RedirectToAction("Details", new { id = _newTask.TaskID });

            }
            return View();
        }

        // GET: TaskController/Edit/5
        [HttpGet]
        [Route("Edit")]
        public ActionResult Edit(int id)
        {
            ToDoTask _newTask = _taskRepository.Details(id);

            if (_newTask == null)
            {
                Response.StatusCode = 404;
                return View("TaskNotFound", id);
            }

            TaskCreateViewModel taskCreateViewModel = new TaskCreateViewModel()
            {
                TaskID = _newTask.TaskID,
                TaskName = _newTask.TaskName,
                TaskDescription= _newTask.TaskDescription,
                TaskActive=_newTask.TaskActive
            };

            return View("~/Views/Task/Edit.cshtml", taskCreateViewModel);
        }

        // POST: TaskController/Edit/5
        [HttpPost]
        [Route("Edit")]
        public ActionResult Edit(TaskCreateViewModel model)
        {
            try
            {
                ToDoTask _newTask = new ToDoTask();
                _newTask = _taskRepository.Details(model.TaskID);
                if (ModelState.IsValid && _newTask != null)
                {
                    _newTask.TaskID = model.TaskID;
                    _newTask.TaskName = model.TaskName;
                    _newTask.TaskDescription = model.TaskDescription;
                    _newTask.TaskActive = model.TaskActive;
                }
                _taskRepository.Update(_newTask);
                //return RedirectToAction("Details", model.TaskID);
                return RedirectToAction("Details", new { id = model.TaskID });
            }
            catch
            {
                return View();
            }
        }

 



        // GET: TaskController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TaskController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
