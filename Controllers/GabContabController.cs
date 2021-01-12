using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoList.Models;
using toDoList.Models.Interfaces;
using toDoList.Models.SQL;
using toDoList.ViewModels;
using System.Text.Json;
using System.Text.Json.Serialization;
using toDoList.Helpers;

namespace toDoList.Controllers
{
    public class GabContabController : Controller
    {
        private readonly AppDbContext context;
        // GET: GabContabController
        public GabContabController(AppDbContext _context)
        {
            context = _context;
        }

        public ActionResult Index()
        {
            GabineteEditViewModel model = new GabineteEditViewModel();
            GabContabilidadeRepository gabContabilidade = new GabContabilidadeRepository(context);
            model.EmpresasContabilidade = gabContabilidade.GetGabContabilidade();
            model.Empresas = gabContabilidade.GetEmprGabContabilidade(0);
            model.AnoFiscal = gabContabilidade.GetEmprGabContabilidadeAno(0);
            return this.PartialView("~/Views/Gabinete/Create.cshtml", model);
        }

        [HttpGet]
        public IActionResult GetGabContab()
        {
            GabContabilidadeRepository gabContabilidade = new GabContabilidadeRepository(context);
            IEnumerable<SelectListItem> gabContab = gabContabilidade.GetGabContabilidade();
            ViewBag.ListOfGabinetes(gabContab.ToList<SelectListItem>());
            return View();
        }
  

        [HttpGet]
        public string GetEmprContab(int EmpresaID)
        {
             if (EmpresaID >= 0)
            {
                GabContabilidadeRepository gabContabilidade = new GabContabilidadeRepository(context);
                IEnumerable<SelectListItem> emprGabContab = gabContabilidade.GetEmprGabContabilidade(EmpresaID);

                SessionHelper.SetObjectAsJson(HttpContext.Session, "idGabContab", EmpresaID.ToString());
           
                return JsonSerializer.Serialize(emprGabContab);
            }
            return null;
        }

        [HttpGet]
        public string GetAnoEmprContab(int EmpresaID)
        {
            if (EmpresaID >= 0)
            {
                GabContabilidadeRepository gabContabilidade = new GabContabilidadeRepository(context);
                IEnumerable<SelectListItem> emprGabContabAno = gabContabilidade.GetEmprGabContabilidadeAno(EmpresaID);
                
                SessionHelper.SetObjectAsJson(HttpContext.Session, "idEmpresaContab", EmpresaID.ToString());
                return JsonSerializer.Serialize(emprGabContabAno);
            }
            return null;
        }
        
        [HttpGet]
        public string SaveSessionAnoEmprContab(string AnoSelectionado) {
            SessionHelper.SetObjectAsJson(HttpContext.Session, "idAnoEmpresaContab", AnoSelectionado.ToString());

            int idEmpresaContab = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "idEmpresaContab");

            GabContabilidadeRepository gabContabilidade = new GabContabilidadeRepository(context);
            DadosEmpresaImportada empVmodel = gabContabilidade.GetEmpresaModel(idEmpresaContab,Int16.Parse(AnoSelectionado));
            
            SessionHelper.SetObjectAsJson(HttpContext.Session, "CodeEmpresa", empVmodel.CodeEmpresa.ToString());

            return JsonSerializer.Serialize(AnoSelectionado);
        }
        // GET: GabContabController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: GabContabController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GabContabController/Create
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

        // GET: GabContabController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: GabContabController/Edit/5
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

        // GET: GabContabController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: GabContabController/Delete/5
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
