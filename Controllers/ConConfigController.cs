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

        public IActionResult Index(int EmpresaId)
        {
            IEnumerable<EmpresasViewModel> modelEmpresa = empresa.GetModelByID(EmpresaId);

            IEnumerable<ConConfigViewModel> conConfigViewModels = conConfig.GetExistingRegistries(EmpresaId);
            
            EmpresasViewModel _model = modelEmpresa.ToList()[0];
            ViewBag.NomeEmpresa = _model.Nome;
            ViewBag.EmpresaID = _model.EmpresaID;
            return View("~/Views/ConConfig/Index.cshtml", conConfigViewModels);

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
                ConexaoID = _conConf.ConexaoID,
                InstanciaSQL = _conConf.InstanciaSQL,
                NomeServidor = _conConf.NomeServidor,
                ActiveConnection = _conConf.ActiveConnection,
                Password = _conConf.Password,
                Utilizador = _conConf.Utilizador
            };

            ViewBag.NomeServidor = model.NomeServidor;
            return View(model);
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
                return RedirectToAction("Index");
            }
            ViewBag.Signal = "notok";
            ViewBag.ErrorTitle = "Erro ao apagar!";
            ViewBag.ErrorMessage = $"Não foi possível apagar a conexao {_model.NomeServidor}, " +
                                   " se o erro persistir, entre em contato com o suporte!";
            return View("~/Views/Error/GeneralError.cshtml");
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
            return View(_tmpModel);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ConConfigViewModel _tmpModel = conConfig.GetModelByID(id);
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

            ViewBag.NomeServidor = model.NomeServidor;
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(ConConfigViewModel _model)
        {

            if (_model == null)
            {
                ViewBag.ErrorMessage = $"Modelo da conexao = {_model.NomeServidor} nao foi possivel de encontrar";
                return View("NotFound");
            }
            else
            {

                bool bResult = conConfig.UpdateConnection(_model);//
                if (bResult)
                {
                    return RedirectToAction("Index", new { EmpresaId = _model.EmpresaId});
                }
                ViewBag.Signal = "notok";
                ViewBag.ErrorTitle = "Erro de atualização!";
                ViewBag.ErrorMessage = "Não foi possível atualizar os dados de conexão no servidor, " +
                                       " se o erro persistir, entre em contato com o suporte!";
                return View("~/Views/Error/GeneralError.cshtml");
            }

        }

        [HttpGet]
        public IActionResult CreateConConfig(int EmpresaId)
        {
            EmpresasViewModel modelEmpresa = empresa.GetModelByID(EmpresaId).ToList()[0];
            ViewBag.NomeEmpresa = modelEmpresa.Nome;
            ViewBag.HostName = ReturnHostName();
            ViewBag.EmpresaId = EmpresaId;
            return View();
        }

        [HttpPost]
        public IActionResult CreateConConfig(int EmpresaId, ConConfigViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.EmpresaId = EmpresaId;
                bool result = conConfig.DeleteConnection(model);

                if (result)
                {
                    result = conConfig.InsertConnection(model);
                    if (result)
                    {
                        IEnumerable<ConConfigViewModel> _mdl = conConfig.GetExistingRegistries(EmpresaId);
                        return View("~/Views/ConConfig/Index.cshtml", _mdl);
                        //return RedirectToAction("Index", "ConConfig", EmpresaId);
                    }
                    else
                    {
                        ViewBag.Signal = "notok";
                        ViewBag.ErrorTitle = "Erro de inserção";
                        ViewBag.ErrorMessage = "Não foi possível inserir os dados de conexão no servidor, " +
                                               "se o erro persistir entre em contato com o suporte!";
                        return View("~/Views/Error/GeneralError.cshtml");
                    }
                }
                else
                {
                    ViewBag.Signal = "notok";
                    ViewBag.ErrorTitle = "Erro de inserção";
                    ViewBag.ErrorMessage = "Não foi possível inserir os dados de conexão no servidor, " +
                                           "se o erro persistir entre em contato com o suporte!";
                    return View("~/Views/Error/GeneralError.cshtml");
                }
            }
            return View(model);
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
