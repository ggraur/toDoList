using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.Controllers
{
    public class CLabController : Controller
    {
        // GET: CLabController
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //// GET: CLabController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: CLabController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CLabController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: CLabController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CLabController/Edit/5
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

        // GET: CLabController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CLabController/Delete/5
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
