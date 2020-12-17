using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoList.Models;
using toDoList.ViewModels;

namespace toDoList.Controllers
{
    public class AGesController : Controller
    {
        private readonly AGesContext agesContext;
        private readonly IEmpresa empContext;
        private readonly IEmpresaAGes empresaAGes;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<AccountController> logger;

        public AGesController(AGesContext _agesContext, IEmpresa _empContext, 
                IEmpresaAGes _empresaAGes, UserManager<ApplicationUser> userManager, ILogger<AccountController> _logger)
        {
            this.agesContext = _agesContext;
            this.empContext = _empContext;
            this.empresaAGes = _empresaAGes;
            this.userManager = userManager;
            this.logger = _logger;
        }
        // GET: AGesController
        public ActionResult Index()
        {
            IEnumerable<Empr> model = agesContext.Emprs.ToList();
            List<EmprViewModel> tmp = new List<EmprViewModel>();
            foreach (Empr empr in model)
            {
                EmprViewModel _tmp = new EmprViewModel()
                {
                    Actividade = empr.Actividade,
                    Cae = empr.Cae,
                    CapSocial = empr.CapSocial,
                    Cemp = empr.Cemp,
                    CodPostal = empr.CodPostal,
                    Concelho = empr.Concelho,
                    ConsRegC = empr.ConsRegC,
                    Distrito = empr.Distrito,
                    Email = empr.Email,
                    Fax = empr.Fax,
                    Freguesia = empr.Freguesia,
                    IsSelected = false,
                    Localidade = empr.Localidade,
                    Matriculado = empr.Matriculado,
                    Morada = empr.Morada,
                    Ncontrib = empr.Ncontrib,
                    Nome = empr.Nome,
                    RfinancasC = empr.RfinancasC,
                    RfinancasD = empr.RfinancasD,
                    Sede = empr.Sede,
                    Telefone = empr.Telefone


                };

                tmp.Add(_tmp);

            }

            if (tmp != null)
            {
                return View("~/Views/AGes/index.cshtml", tmp);
            }
            @ViewBag.Signal = "notok";
            @ViewBag.ErrorTitle = "Erro de contexto / conexão Db!";
            @ViewBag.ErrorMessage = "Não foi possível devolver o modelo das empresas DB AGes!";
            return View("~/Views/Error/GeneralError.cshtml");

        }

        // GET: AGesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AGesController/Create
        public ActionResult Import()
        {
            return View();
        }

        //POST: AGesController/Create
        [HttpPost]

        public async Task<ActionResult> ImportAsync(List<EmprViewModel> model)
        {
            bool bResult = false;
            try
            {
                List<EmpresasViewModel> cabContab = empContext.GetActiveCabContabilidade().ToList();
                if (cabContab.Count() == 0)
                {
                    return RedirectToAction("CreateCabContabilidade");
                }
                List<EmpresasViewModel> lstTmp = new List<EmpresasViewModel>();
                foreach (EmprViewModel itm in model)
                {
                    if (itm.Ncontrib != null && itm.Ncontrib != "")
                    {
                        EmpresasViewModel tmp = new EmpresasViewModel();
                        tmp.isCabContabilidade = false;
                        tmp.IdCabContabilidade = cabContab[0].EmpresaID;
                        tmp.Ativo = true;
                        tmp.NIF = itm.Ncontrib;
                        tmp.Nome = itm.Nome;
                        tmp.Licenca = "";
                        tmp.DataCriacao = DateTime.Now;
                        tmp.DataExpiracao = DateTime.Now.AddYears(1);
                        tmp.NrEmpresas = 0;
                        tmp.NrPostos = "0";
                        bResult = empContext.InsertEmpresa(tmp);
                        
                        if (bResult == true)
                        {// add users  
                            int tmpEmpresaID = empContext.ReturnCompanyID(tmp.Nome,tmp.NIF);
                            try
                            {
                                IEnumerable<AGesEmpresasUtilizadores> _emprUtil = empresaAGes.GetUtilizadores(tmp.NIF, tmp.Nome);
                                List<string> _utls = (from s in _emprUtil
                                                      select s.Utilizador)
                                                     .Distinct().ToList();
                                foreach (string s in _utls)
                                {
                                    //Se o utilizador nao existir na base de dados cadastrar se existir so aditionar a empresa em causa.
                                    ApplicationUser usrApp = await userManager.FindByNameAsync( s + "@gestecnica.com");
                                    if (usrApp == null)
                                    { //insert new user
                                        ApplicationUser user = new ApplicationUser
                                        {
                                            UserName = s + "@gestecnica.com",
                                            Email = s + "@gestecnica.com"
                                        };
                                        var result = await userManager.CreateAsync(user, "@Gestecnica_com!2020");
                                        logger.Log(LogLevel.Warning, DateTime.Now.ToString() + $": New user '{user.UserName}' successfully added automatically ");
                                        usrApp = await userManager.FindByNameAsync(s + "@gestecnica.com");
                                    }
                                    else
                                    {
                                        logger.Log(LogLevel.Warning, DateTime.Now.ToString() + $": User '{usrApp.UserName}' already exist");
                                    }
                                    /* adding user as specific user to specific company*/
                                 bool sRlt1 = await AddUserToSpecificCompanyAsync(usrApp, tmpEmpresaID);
                                }
                            }
                            catch (Exception ex)
                            {
                                logger.Log(LogLevel.Warning, ex.Message);
                                Console.Write(ex.Message);
                            }
                        }
                        else
                        {
                            @ViewBag.Signal = "notok";
                            @ViewBag.ErrorTitle = "Erro de insercao na DB!";
                            @ViewBag.ErrorMessage = $"Não foi possível inserir a empresa com o numero de contribuinte {tmp.NIF}, nome = {tmp.Nome}";
                            return View("~/Views/Error/GeneralError.cshtml");
                        }
                    }
                }

                return RedirectToAction("Index", "Empresa");
            }
            catch
            {
                return RedirectToAction("Index", "AGes");
                // return View();
            }
        }

        private async Task<bool> AddUserToSpecificCompanyAsync(ApplicationUser user, int empresaId)
        {
            bool bResult = false;

           List<EmpresaUtilizadoresViewModel> tmpUser = empContext.GetUtilizadorEmpresa(user.UserName,empresaId).ToList(); 

            if (tmpUser.Count == 1)
            {
                bResult = true;
                return bResult;
                //bool result = await empContext.RemoveUtilizadorFromEmpresaAsync(tmpUser[0]);
            }

            EmpresaUtilizadoresViewModel tmp1 = new EmpresaUtilizadoresViewModel()
            {
                EmpresaId = empresaId,
                IsSelected =true,
                UserID = user.Id,
                UserName = user.UserName
            };

            bool result1 = await empContext.AddUtilizadorEmpresaAsync(empresaId, tmp1);

            if (result1 == true)
            {
                logger.Log(LogLevel.Warning, $"Usuário: {user.UserName} adicionado como utilizador da empresa com ID: {tmp1.EmpresaId}");
                bResult = true;
            }
            else
            {
                logger.Log(LogLevel.Warning, $"Erro ao tentar adicionar Usuário: {user.UserName} como utilizador da empresa com ID: {tmp1.EmpresaId}");
                bResult = false;
            }

            return bResult;
        }

        // GET: AGesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AGesController/Edit/5
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

        // GET: AGesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AGesController/Delete/5
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
