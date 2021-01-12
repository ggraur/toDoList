using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.Controllers
{
    public class TipoDocumentosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
