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
    public class ToDoListController : Controller
    {
        private readonly ITaskRepository _taskRepository;

        // GET: ToDoListController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ToDoListController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ToDoListController/Create
        public ActionResult Create()
        {
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
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ToDoListController/Delete/5
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
