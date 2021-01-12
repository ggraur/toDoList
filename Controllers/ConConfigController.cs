using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using toDoList.ViewModels;
using toDoList.Models;

namespace toDoList.Controllers
{
    [Authorize]
    public class ConConfigController : Controller
    {
        private readonly IConConfig conConfig;
        private readonly IEmpresa empresa;

        public ConConfigController(IConConfig _conConfig, IEmpresa _empresa)
        {
            this.conConfig = _conConfig;
            this.empresa = _empresa;
        }
        [HttpGet]
        public IActionResult Index(int EmpresaId)
        {
            IEnumerable<EmpresasViewModel> modelEmpresa = empresa.GetModelByID(EmpresaId);
            IEnumerable<ConConfigViewModel> conConfigViewModels = conConfig.GetExistingRegistries(EmpresaId);

            EmpresasViewModel _model = modelEmpresa.ToList()[0];
            ViewBag.NomeEmpresa = _model.Nome;
            ViewBag.EmpresaID = _model.EmpresaID;
            return this.PartialView("~/Views/ConConfig/Index.cshtml", conConfigViewModels);

            //IEnumerable<ConConfigViewModel> model =(IEnumerable<ConConfigViewModel>)conConfig.GetExistingRegistries(EmpresaID);
            //return RedirectToAction("CreateConConfig", "ConConfig");
        }
        [HttpGet]
        public IActionResult IndexJson(int Id)
        {
            int EmpresaId = Id;
            IEnumerable<EmpresasViewModel> modelEmpresa = empresa.GetModelByID(EmpresaId);
            IEnumerable<ConConfigViewModel> conConfigViewModels = conConfig.GetExistingRegistries(EmpresaId);

            EmpresasViewModel _model = modelEmpresa.ToList()[0];
            ViewBag.NomeEmpresa = _model.Nome;
            ViewBag.EmpresaID = _model.EmpresaID;
            return this.PartialView("~/Views/ConConfig/Index.cshtml", conConfigViewModels);

            //IEnumerable<ConConfigViewModel> model =(IEnumerable<ConConfigViewModel>)conConfig.GetExistingRegistries(EmpresaID);
            //return RedirectToAction("CreateConConfig", "ConConfig");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            ConConfigViewModel _conConf = conConfig.GetModelByID(id);
            if (_conConf == null)
            {
                ViewBag.ErrorMessage = $"A conexão com ID = {id} não foi possível de encontrar .";
                return View("NotFound");
            }
            var model = new ConConfigViewModel
            {
                EmpresaId = _conConf.EmpresaId,
                ConexaoID = _conConf.ConexaoID,
                InstanciaSQL = _conConf.InstanciaSQL,
                NomeServidor = _conConf.NomeServidor,
                ActiveConnection = _conConf.ActiveConnection,
                Password = _conConf.Password,
                Utilizador = _conConf.Utilizador
            };

            ViewBag.NomeServidor = model.NomeServidor;
            return this.PartialView(model);
        }

        [HttpPost]
        public IActionResult Delete(ConConfigViewModel _model)
        {
            ConConfigViewModel _tmpModel = conConfig.GetModelByID(_model.ConexaoID);
            if (_tmpModel == null)
            {
                ViewBag.ErrorMessage = $"A conexão com ID = {_model.NomeServidor} não foi possível de encontrar .";
                return View("NotFound");
            }

            bool bResult = conConfig.DeleteConnection(_model);
            if (bResult)
            {
                return RedirectToAction("Index", new { EmpresaId = _tmpModel.EmpresaId });
            }
            ViewBag.Signal = "notok";
            ViewBag.ErrorTitle = "Erro ao apagar!";
            ViewBag.ErrorMessage = $"Não foi possível apagar a conexao {_model.NomeServidor}, " +
                                   " se o erro persistir, entre em contato com o suporte!";
            return this.PartialView("~/Views/Error/GeneralError.cshtml");
        }

        [HttpPost]
        public IActionResult DeleteJson(int id)
        {
            IEnumerable<ConConfigViewModel> _tmpModel = conConfig.IenumGetModelByID(id);
            if (_tmpModel.ToList().Count == 0)
            {
                ViewBag.ErrorMessage = $"A conexão com ID = {id} não foi possível de encontrar .";
                return View("NotFound");
            }
            else if (_tmpModel.ToList().Count == 1)
            {
                bool bResult = conConfig.DeleteConnection(_tmpModel.ToList()[0]);
                if (bResult)
                {
                    return Json(new { success = true });
                }

            }

            return Json(new { success = false });

            //ViewBag.Signal = "notok";
            //ViewBag.ErrorTitle = "Erro ao apagar!";
            //ViewBag.ErrorMessage = $"Não foi possível apagar a conexao {_tmpModel.NomeServidor}, " +
            //                       " se o erro persistir, entre em contato com o suporte!";
            //return this.PartialView("~/Views/Error/GeneralError.cshtml");
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            ConConfigViewModel _tmpModel = conConfig.GetModelByID(id);
            if (_tmpModel == null)
            {
                ViewBag.ErrorMessage = $"A conexão com ID = {_tmpModel.NomeServidor} não foi possível de encontrar .";
                return View("NotFound");
            }

            ViewBag.NomeServidor = _tmpModel.NomeServidor;
            return this.PartialView(_tmpModel);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ConConfigViewModel _tmpModel = conConfig.GetModelByID(id);
            IEnumerable<EmpresasViewModel> vmEmpresa = empresa.GetModelByID(_tmpModel.EmpresaId);

            if (_tmpModel == null)
            {
                ViewBag.ErrorMessage = $"A conexão com ID = {_tmpModel.NomeServidor} não foi possível de encontrar .";
                return View("NotFound");
            }
            var model = new ConConfigViewModel
            {
                ConexaoID = _tmpModel.ConexaoID,
                EmpresaId = _tmpModel.EmpresaId,
                InstanciaSQL = _tmpModel.InstanciaSQL,
                NomeServidor = _tmpModel.NomeServidor,
                ActiveConnection = _tmpModel.ActiveConnection,
                Password = _tmpModel.Password,
                Utilizador = _tmpModel.Utilizador
            };
            //ver se exite a empressa e nome empres tem que se passar aqui
            ViewBag.NomeServidor = model.NomeServidor;
            if (vmEmpresa.ToList().Count > 0)
            {
                ViewBag.NomeEmpresa = vmEmpresa.ToList()[0].Nome;
            }

            return this.PartialView(model);
        }

        [HttpPost]
        public IActionResult Edit([FromBody] ConConfigViewModel _model)
        {
            if (_model == null)
            {
                ViewBag.ErrorMessage = $"Modelo da conexao = {_model.NomeServidor} nao foi possivel de encontrar";
                return View("NotFound");
            }
            else
            {
                string bResult = conConfig.UpdateConnection(_model);//
                if (bResult.StartsWith("success:true"))
                {
                    //IEnumerable<ConConfigViewModel> _m = conConfig.GetExistingRegistries(_model.EmpresaId);
                    //return this.PartialView("~/Views/ConConfig/Index.cshtml",_m);
                    return Json(new { success = true, msg = "Successful operation" });
                    //return RedirectToAction("Index", new { EmpresaId = _model.EmpresaId});
                }

                if (bResult.StartsWith("success:false"))
                {
                    List<string> d = bResult.Split(",").ToList();

                    return Json(new { success = false, msg = d[1].ToString() });
                }
                // ViewBag.Signal = "notok";
                //ViewBag.ErrorTitle = "Erro de atualização!";
                //ViewBag.ErrorMessage = "Não foi possível atualizar os dados de conexão no servidor, " +
                //                       " se o erro persistir, entre em contato com o suporte!";
                //return this.PartialView("~/Views/Error/GeneralError.cshtml");
                return View();
            }

        }


        [HttpPost]
        public IActionResult EditJson(int ConexaoID, int EmpresaId, string NomeServidor, string InstanciaSQL, string Utilizador, string Password, bool ActiveConnection)
        {
            //https://stackoverflow.com/questions/40682403/bad-request-for-jquery-ajax-to-post-stringified-json-data-to-mvc-action
            ConConfigViewModel _model = new ConConfigViewModel
            {
                ConexaoID = ConexaoID,
                EmpresaId = EmpresaId,
                NomeServidor = NomeServidor,
                InstanciaSQL = InstanciaSQL,
                Utilizador = Utilizador,
                Password = Password,
                ActiveConnection = ActiveConnection
            };

            string bResult = conConfig.UpdateConnection(_model);//

            List<string> d = bResult.Split("|").ToList();
            List<string> v = d[0].ToString().Split(":").ToList();

            var s = Json(new { success = bool.Parse(v[1].ToString()), msg = d[1].ToString().Replace("'", ""), field = d[2].ToString() });

            return s;

        }

        [HttpGet]
        public IActionResult CreateConConfig(int id)
        {
            EmpresasViewModel modelEmpresa = empresa.GetModelByID(id).ToList()[0];

            if (id > 0)
            {
                IEnumerable<ConConfigViewModel> conConfigs = conConfig.GetExistingRegistries(id);
                if (conConfigs.ToList().Count != 0)
                {
                    ViewBag.EmpresaID = id;
                    ViewBag.NomeEmpresa = modelEmpresa.Nome;
                    return this.PartialView("~/Views/ConConfig/index.cshtml", conConfigs);
                }
            }

            //GetExistingRegistries
            //nao esta correcto se ja existir uma conexao, entao caregar


            ViewBag.NomeEmpresa = modelEmpresa.Nome;
            ViewBag.HostName = ReturnHostName();
            ViewBag.EmpresaId = id;
            return this.PartialView();
        }

        [HttpPost]
        public IActionResult CreateConConfig(int EmpresaId, ConConfigViewModel model)
        {
            bool result = false;
            if (ModelState.IsValid)
            {
                IEnumerable<ConConfigViewModel> tmp = conConfig.FindByEmpresaId(EmpresaId);

                if (tmp.ToList().Count > 0)
                {
                    model.EmpresaId = EmpresaId;
                    result = conConfig.DeleteConnection(model);
                }

                //result = conConfig.InsertConnection(model);
                string bResult = conConfig.InsertConnection(model);

                List<string> d = bResult.Split("|").ToList();
                List<string> v = d[0].ToString().Split(":").ToList();

                var s = Json(new { success = bool.Parse(v[1].ToString()), msg = d[1].ToString().Replace("'", ""), field = d[2].ToString() });

                return s;
                //if (result)
                //{
                //    IEnumerable<ConConfigViewModel> _mdl = conConfig.GetExistingRegistries(EmpresaId);
                //    return this.PartialView("~/Views/ConConfig/Index.cshtml", _mdl);
                //}
                //else
                //{
                //    ViewBag.Signal = "notok";
                //    ViewBag.ErrorTitle = "Erro de inserção";
                //    ViewBag.ErrorMessage = "Não foi possível inserir os dados de conexão no servidor, " +
                //                           "se o erro persistir entre em contato com o suporte!";
                //    return this.PartialView("~/Views/Error/GeneralError.cshtml");
                //}
            }
            return this.PartialView(model);
        }


        [HttpPost]
        public IActionResult CreateJson(ConConfigViewModel model)
        {
            int EmpresaId = model.EmpresaId;
            bool result = false;

            IEnumerable<ConConfigViewModel> tmp = conConfig.FindByEmpresaId(EmpresaId);

            if (tmp.ToList().Count > 0)
            {
                model.EmpresaId = EmpresaId;
                result = conConfig.DeleteConnection(model);
            }
            string bResult = conConfig.InsertConnection(model);

            List<string> d = bResult.Split("|").ToList();
            List<string> v = d[0].ToString().Split(":").ToList();

            var s = Json(new { success = bool.Parse(v[1].ToString()), msg = d[1].ToString().Replace("'", ""), field = d[2].ToString() });

            return s;
        }


        private string ReturnHostName()
        {
            string sResult = "";

            string hostName = Dns.GetHostName();
            IPAddress[] addresses = Dns.GetHostAddresses(hostName);

            sResult = "HostName: " + hostName + " | IP: " + addresses[1].ToString();

            return sResult;

        }

    }
}
