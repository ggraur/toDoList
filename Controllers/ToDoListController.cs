using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoClassLibrary;
using toDoList.Models;
using toDoList.ViewModels;

namespace toDoList.Controllers
{
    [Route("[controller]")]
    public class ToDoListController : Controller
    {

        private IConfiguration _config;

        private readonly IToDoListRepository _toDoListRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITaskRepository _taskRepository;
        public ToDoListController(IConfiguration config,
                                  IToDoListRepository toDoRepository,
                                  IUserRepository userRepository,
                                  ITaskRepository taskRepository)
        {
            _config = config;
            this._toDoListRepository = toDoRepository;
            this._userRepository = userRepository;
            this._taskRepository = taskRepository;
        }
        // GET: ToDoListController
        [HttpGet]
        [Route("Index")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("Index/{id?}")]
        public ActionResult Index(int id)
        {
            return RedirectToAction("ShowAllListsToDo", new { id = id });
       
        }

        // GET: ToDoListController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpGet]
        [Route("AddTask")]
        public ActionResult AddTask()
        {
            return View("~/Views/ToDoList/AddTask.cshtml");
        }


        [HttpGet]
        [Route("Create/{id?}")]
        // GET: ToDoListController/Create
        public ActionResult Create(int id)
        {
            var _tmpUser = _userRepository.GetUserDetails(id);
            if (_tmpUser != null)
            {
                ToDoList tmp_toDoList = _toDoListRepository.Add(
                        new ToDoList
                        {
                            ToDoListName = _tmpUser.UserName + "'s To Do List",
                            UserIDCreator = _config["LogedUser"],
                            IDCreator = Convert.ToInt32(_config["LogedUserID"].ToString()),
                            UserIDExecutor = _tmpUser.UserName,
                            IDExecutor = _tmpUser.UserID,
                            CreatedToDoListDatetime = DateTime.Now,
                            FinalizationDatetime = DateTime.Now.AddDays(1)
                        }
                     );

                IEnumerable<ToDoTask> tasks = _taskRepository.ActiveTasks();
                IEnumerable<AddTask_To_ToDoList> c = _toDoListRepository.AddTasksToList(tasks.ToList(), tmp_toDoList);
                IEnumerable<AddTask_To_ToDoList> model = _toDoListRepository.GetToDoListById(tmp_toDoList.ToDoListID);
                ViewBag.ListName = tmp_toDoList.ToDoListName;
                ViewBag.AssignedTo = tmp_toDoList.UserIDExecutor;
                ViewBag.AssignedBy = tmp_toDoList.UserIDCreator;
                c = _toDoListRepository.GetListByAssignedUserId(tmp_toDoList.IDExecutor);
                return View("~/Views/ToDoList/ShowActiveListWithToDoTasks.cshtml", c);
            }
            return View("Index");
        }

        [HttpGet]
        [Route("ShowAllListsToDo/{id?}")]
        public ActionResult ShowAllListsToDo(int id)
        {

            IEnumerable<AddTask_To_ToDoList> c = _toDoListRepository.GetListByAssignedUserId(id);
            //ViewBag.AssignedTo = c.ToList()[0]..UserIDExecutor;
            return View("~/Views/ToDoList/ShowActiveListWithToDoTasks.cshtml", c);
        }

        //[HttpGet]
        //[Route("ShowTask/{id?}")]
        //public ActionResult ShowTask(int id)
        //{
        //    IEnumerable<ToDoList>  _toDoList = _toDoListRepository.GetListById(id);

        //    ToDoList _tmptoDoList = new ToDoList();



        //                ToDoTask toDoTask = new ToDoTask() { 

        //    };

        //    return View();
        //}
        //[HttpPost]
        //[Route("EditTask/{id?}")]
        //public ActionResult EditTask(int id)
        //{

        //    return View("~/Views/ToDoList/Index.cshtml");
        //}
        [HttpGet]
        [Route("EditTask/{id?}")]
        public ActionResult EditTask(int id)
        {
            IEnumerable<AddTask_To_ToDoList> addTask_To_ToDo = _toDoListRepository.GetToDoListById(id);

            return View("~/Views/ToDoList/Edit.cshtml", addTask_To_ToDo.ToList()[0]);
        }

        [HttpPost]
        [Route("EditTask")]
        public ActionResult EditTask()
        {
           // IEnumerable<AddTask_To_ToDoList> addTask_To_ToDo = _toDoListRepository.GetToDoListById(id);

            return View();
        }
        // POST: ToDoListController/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Route("Save")]
        public ActionResult Save(ToDoList toDoList)
        {
            List<ToDoList> lst = _toDoListRepository.GetListById(toDoList.ToDoListID).ToList();

            if (lst.Count == 1)
            {
                try
                {
                    IEnumerable<ToDoList> allToDoList = _toDoListRepository.GetListByCreatorByAssignedToUser(lst[0].IDCreator, lst[0].IDExecutor);
                    return View("~/Views/ToDoList/Index.cshtml", allToDoList);
                }
                catch
                {
                    return View();
                }
            }
            return View();

        }

        // GET: ToDoListController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ToDoListController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: ToDoListController/Delete/5

        [HttpGet]
        [Route("Delete/{id?}")]
        public ActionResult Delete(int id)
        {

            IEnumerable<AddTask_To_ToDoList> toDoTask = _toDoListRepository.GetListItemByIdItem(id);
            //AddTask_To_ToDoList toDoTask = _toDoListRepository.GetListItemByIdItem(id).ToList()[0];
            List<AddTask_To_ToDoList> lst = toDoTask.ToList();
            if (lst[0] != null)
            {
                return View(lst[0]);
            }
            return View("~/Views/Error/NotFound.cshtml");
        }

        // POST: ToDoListController/Delete/5
        [HttpPost]
        [Route("Delete/{id?}")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                AddTask_To_ToDoList toDoTask = _toDoListRepository.DeleteListItemByIdItem(id);
                //IEnumerable<AddTask_To_ToDoList> model = _toDoListRepository.GetListByAssignedUserId(toDoTask.IDExecutor);
                return RedirectToAction("ShowAllListsToDo", new { id = toDoTask.IDExecutor });
            }
            catch
            {
                return View();
            }
        }
    }
}
