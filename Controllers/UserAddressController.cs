using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class UserAddressController : Controller
    {
        // GET: UserAddressController
        public ActionResult Index()
        {
            return View();
        }

        // GET: UserAddressController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserAddressController/Create

        [Route("Create/{id?}")]
        public ActionResult Create(int? id)
        {
            return View();
        }

        // POST: UserAddressController/Create
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

        // GET: UserAddressController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserAddressController/Edit/5
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

        // GET: UserAddressController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserAddressController/Delete/5
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
