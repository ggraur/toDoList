using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using toDoList.Models;
using toDoList.ViewModels;

namespace toDoList.Controllers
{
    [Authorize]
    public class EmpresaController : Controller
    {
        private readonly IEmpresa context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<EmpresaController> logger;
        public EmpresaController(IEmpresa _context, UserManager<ApplicationUser> _userManager, ILogger<EmpresaController> _logger)
        {
            this.context = _context;
            this.userManager = _userManager;
            this.logger = _logger;
        }

        // GET: EmpresaController
        [HttpGet]
        public ActionResult IndexJson()
        {
            List<EmpresasViewModel> cabContab = context.GetActiveCabContabilidade().ToList();
            if (cabContab.Count() == 0)
            {
                var a = Json(new { success = false });
                return a;
            }
            var b = Json(new { success = true });
            return b;
        }

        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<EmpresasViewModel> empresas = context.GetExistingRegistries();
            return this.PartialView("~/Views/Empresa/Index.cshtml", empresas);
        }


        // GET: EmpresaController/Details/5
        public ActionResult Details(int id)
        {
            EmpresasViewModel tmp = context.GetModelByID(id).ToList()[0];
            ViewBag.NomeEmpresa = tmp.Nome;
            return this.PartialView(tmp);
        }
        [HttpGet]
        // GET: EmpresaController/Create
        public ActionResult Create()
        {
            return this.PartialView();
        }

        // POST: EmpresaController/Create
        [HttpPost]

        public async Task<ActionResult> Create(EmpresasViewModel _model)
        {

            if (ModelState.IsValid)
            {
                var bResult = await context.InsertEmpresa(_model);
                if (bResult)
                {
                    return RedirectToAction("Index", "Empresa");
                }
                else
                {
                    ViewBag.Signal = "notok";
                    ViewBag.ErrorTitle = "Erro de inserção";
                    ViewBag.ErrorMessage = "Não foi possível inserir os dados da Empresa, " +
                                           "se o erro persistir entre em contato com o suporte!";
                    return View("~/Views/Error/GeneralError.cshtml");
                }
            }
            return this.PartialView(_model);

        }

        // GET: EmpresaController/Edit/5

        public ActionResult Edit(int id)
        {
            EmpresasViewModel _tmpModel = context.GetModelByID(id).ToList()[0];
            if (_tmpModel == null)
            {
                ViewBag.ErrorMessage = $"A empresa com ID = {_tmpModel.Nome} não foi possível de encontrar .";
                return View("NotFound");
            }
            var model = new EmpresasViewModel
            {
                EmpresaID = _tmpModel.EmpresaID,
                Nome = _tmpModel.Nome,
                NIF = _tmpModel.NIF,
                Licenca = _tmpModel.Licenca,
                DataCriacao = _tmpModel.DataCriacao,
                DataExpiracao = _tmpModel.DataExpiracao,
                NrEmpresas = _tmpModel.NrEmpresas,
                NrPostos = _tmpModel.NrPostos,
                Ativo = _tmpModel.Ativo
            };

            ViewBag.NomeEmpresa = model.Nome;
            return this.PartialView("~/Views/Empresa/Edit.cshtml", model);

        }

        // POST: EmpresaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                EmpresasViewModel tmp = new EmpresasViewModel
                {
                    EmpresaID = Int32.Parse(collection["EmpresaID"][0].ToString()),
                    NIF = collection["NIF"][0].ToString(),
                    Nome = collection["Nome"][0].ToString(),
                    Licenca = collection["Licenca"][0].ToString(),
                    NrEmpresas = Int32.Parse(collection["NrEmpresas"][0].ToString()),
                    NrPostos = Int32.Parse(collection["NrPostos"][0].ToString()),
                    DataCriacao = DateTime.Parse(collection["DataCriacao"][0]),
                    DataExpiracao = DateTime.Parse(collection["DataExpiracao"][0]),
                    Ativo = bool.Parse(collection["Ativo"][0].ToString())
                };

                bool bResult = context.UpdateEmpresa(tmp);
                if (bResult)
                {
                    return this.PartialView(nameof(Index));
                    // return RedirectToAction(nameof(Index));
                }

                ViewBag.Signal = "notok";
                ViewBag.ErrorTitle = "Erro de atualização!";
                ViewBag.ErrorMessage = "Não foi possível atualizar os dados Empresa, " +
                                       " se o erro persistir, entre em contato com o suporte!";
                return this.PartialView("~/Views/Error/GeneralError.cshtml");

            }
            catch
            {
                return View();
            }
        }

        // GET: EmpresaController/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            IEnumerable<EmpresasViewModel> tmp = context.GetModelByID(id);
            if (tmp == null)
            {
                ViewBag.ErrorMessage = $"A empresa com o ID = {id} não foi possível de encontrar .";
                return View("NotFound");
            }

            ViewBag.NomeEmpresa = tmp.ToList()[0].Nome;
            return this.PartialView(tmp.ToList()[0]);
        }

        // POST: EmpresaController/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, IFormCollection collection)
        {

            IEnumerable<EmpresasViewModel> tmp = context.GetModelByID(id);
            if (tmp == null)
            {
                ViewBag.ErrorMessage = $"A empresa com ID = {id} não foi possível de encontrar .";
                return View("NotFound");
            }
            bool bResult = context.DeleteEmpresa(tmp.ToList()[0]);
            if (bResult)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Signal = "notok";
            ViewBag.ErrorTitle = "Erro ao apagar!";
            ViewBag.ErrorMessage = $"Não foi possível apagar a empresa {tmp.ToList()[0].Nome}, " +
                                   " se o erro persistir, entre em contato com o suporte!";
            return this.PartialView("~/Views/Error/GeneralError.cshtml");

        }

        [HttpGet]
        public async Task<IActionResult> ManageEmpresaUtilizadoresAsync(int id)
        {
            ViewBag.empresaId = id;
            IEnumerable<EmpresasViewModel> empresa = context.GetModelByID(id);
            if (empresa == null)
            {
                ViewBag.ErrorMessage = $"Empresa com Id = {id} nao foi possivel de encontrar";
                return View("NotFound");
            }

            var model = new List<EmpresaUtilizadoresViewModel>();

            foreach (var user in userManager.Users)
            {
                var empresaUtilizadorViewModel = new EmpresaUtilizadoresViewModel
                {
                    UserID = user.Id,
                    UserName = user.UserName
                };

                if (await context.IsCompanyUserAsync(id, user.UserName))
                {
                    empresaUtilizadorViewModel.IsSelected = true;
                }
                else
                {
                    empresaUtilizadorViewModel.IsSelected = false;
                }
                model.Add(empresaUtilizadorViewModel);
            }
            ViewBag.EmpresaId = empresa.ToList()[0].EmpresaID;
            ViewBag.EmpresaNome = empresa.ToList()[0].Nome;
            return this.PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageEmpresaUtilizadoresAsync(List<EmpresaUtilizadoresViewModel> model, int EmpresaId)
        {
            IEnumerable<EmpresasViewModel> empresa = context.GetModelByID(EmpresaId);

            if (empresa == null)
            {
                ViewBag.ErrorMessage = $"A empresa com Id = {EmpresaId} não foi encontrada";
                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                model[i].EmpresaId = EmpresaId;
            }

            ViewBag.EmpresaId = EmpresaId;
            ViewBag.EmpresaNome = empresa.ToList()[0].Nome;

            List<ApplicationUser> users = context.GetUtilizadoresEmpresa(EmpresaId);// .EmpresaUtilizadores. ;

            if (users.Count > 0)
            {
                try
                {
                    bool result = await context.RemoveFromUtilizadoresEmpresaAsync(EmpresaId);
                }
                catch (DbException ex)
                {
                    logger.Log(LogLevel.Warning, ex.Message);
                }
            }

            bool result1 = await context.AddUtilizadoresEmpresaAsync(EmpresaId, model.Where(x => x.IsSelected == true).ToList<EmpresaUtilizadoresViewModel>());

            if (result1 != true)
            {
                ModelState.AddModelError("", "Não foi possível remover usuários da empresa");
                return View(model);
            }

            return RedirectToAction("Index", "Empresa", new { Id = EmpresaId });
        }

        [HttpGet]
        public IActionResult CreateCabContabilidade()
        {
            return PartialView();
        }

        [HttpPost]
        public IActionResult CreateCabContabilidadeJson(string nomeEmpresa, string nifEmpresa, string licencaEmp, string nrPostLicEmp, string nrEmpresasEmp, string dtExpLicEmp)
        {
            EmpresasViewModel model = new EmpresasViewModel
            {
                Nome = nomeEmpresa,
                NIF = nifEmpresa,
                Licenca = licencaEmp
            };
            if (nrPostLicEmp != null)
            {
                model.NrPostos = Int32.Parse(nrPostLicEmp);
            };
            if (nrEmpresasEmp != null)
            {
                model.NrEmpresas = Int32.Parse(nrEmpresasEmp);
            };
            if (dtExpLicEmp != null)
            {
                model.DataExpiracao = DateTime.Parse(dtExpLicEmp);
            };
            model.Ativo = true;
            model.DataCriacao = DateTime.Now;
            model.isCabContabilidade = true;
            string bResult = context.InsertEmpresaJson(model);
            List<string> d = bResult.Split("|").ToList();
            List<string> v = d[0].ToString().Split(":").ToList();
            var s = Json(new { success = bool.Parse(v[1].ToString()), msg = d[1].ToString().Replace("'", ""), field = d[2].ToString() });
            return s;
        }

        [HttpPost]
        public IActionResult UpdateCabContabilidadeJson(
            int emresaId, int idCabContabilidade,bool isCabContabilidade,
            string nomeEmpresa, string nifEmpresa, string licencaEmp, 
            string nrPostLicEmp, string nrEmpresasEmp, string dtExpLicEmp)
        {
            EmpresasViewModel model = new EmpresasViewModel
            {
                EmpresaID = emresaId,
                IdCabContabilidade = idCabContabilidade,
                isCabContabilidade = isCabContabilidade,

                Nome = nomeEmpresa,
                NIF = nifEmpresa,
                Licenca = licencaEmp
            };
            if (nrPostLicEmp != null)
            {
                model.NrPostos = Int32.Parse(nrPostLicEmp);
            };
            if (nrEmpresasEmp != null)
            {
                model.NrEmpresas = Int32.Parse(nrEmpresasEmp);
            };
            if (dtExpLicEmp != null)
            {
                model.DataExpiracao = DateTime.Parse(dtExpLicEmp);
            };
            model.Ativo = true;
            model.DataCriacao = DateTime.Now;
            model.isCabContabilidade = true;
            string bResult = context.UpdateEmpresaJson(model);
            List<string> d = bResult.Split("|").ToList();
            List<string> v = d[0].ToString().Split(":").ToList();
            var s = Json(new { success = bool.Parse(v[1].ToString()), msg = d[1].ToString().Replace("'", ""), field = d[2].ToString() });
            return s;
        }


        [HttpPost]
        public async Task<IActionResult> CreateCabContabilidade(EmpresasViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Ativo = true;
                model.DataCriacao = DateTime.Now;
                model.isCabContabilidade = true;
                bool bResult = await context.InsertEmpresa(model);
                if (bResult)
                {
                    return RedirectToAction("Index", "Empresa");
                }
                else
                {
                    ViewBag.Signal = "notok";
                    ViewBag.ErrorTitle = "Erro de inserção";
                    ViewBag.ErrorMessage = "Não foi possível criar o cabinete de contabilidade " +
                                           "se o erro persistir entre em contato com o suporte!";
                    return View("~/Views/Error/GeneralError.cshtml");
                }
            }
            return View(model);
        }
    }
}
