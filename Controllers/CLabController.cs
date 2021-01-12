using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using toDoClassLibrary47;
using toDoList.Helpers;
using toDoList.Models;
using toDoList.ViewModels;
namespace toDoList.Controllers
{
    public class CLabController : Controller
    {

        private readonly IEmpresa empresaContext;
        private readonly AGesContext aGesContext;
        private readonly AppDbContext appDbContext;
        private readonly ILogger<AdministrationController> logger;
        private readonly IWebHostEnvironment env;
        private readonly IDadosEmpresaViewModel dadosEmpresaViewModel;
        private readonly IConConfig conConfig;

        public CLabController(IEmpresa _empresaContext, AGesContext _aGesContext, AppDbContext _appDbContext
            , ILogger<AdministrationController> _logger
            , IWebHostEnvironment _env, IDadosEmpresaViewModel _dadosEmpresaViewModel, IConConfig _conConfig)
        {
            this.empresaContext = _empresaContext;
            this.aGesContext = _aGesContext;
            this.appDbContext = _appDbContext;
            this.logger = _logger;
            this.env = _env;
            this.dadosEmpresaViewModel = _dadosEmpresaViewModel;
            this.conConfig = _conConfig;
        }

        [HttpGet]
        public ActionResult Export()
        {
            int EmpresaID;
            int idGabContab = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "idGabContab");
            int idEmpresaContab = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "idEmpresaContab");
            string AnoEmpresaContab = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "idAnoEmpresaContab");

            if (idGabContab == 0)
            {
                logger.LogError($"Gabinete de contabilidade não foi selecionado!");
                ViewBag.Signal = "notok";
                ViewBag.ErrorTitle = "Gabinete de Contabilidade !";
                ViewBag.ErrorMessage = "Um gabinete de contabilidade não foi selecionado!" +
                                       " Selecione uma empresa de contabilidade para prosseguir com a operação!";
                return this.PartialView("~/Views/Error/GeneralError.cshtml");
            }

            if (idEmpresaContab == 0)
            {
                logger.LogError($"Empresa não foi selecionada!");
                ViewBag.Signal = "notok";
                ViewBag.ErrorTitle = "Empresa não foi selecionada !";
                ViewBag.ErrorMessage = "A Empresa não foi selecionada!" +
                                       " Selecione uma empresa para prosseguir com a operação!";
                return this.PartialView("~/Views/Error/GeneralError.cshtml");
            }
            EmpresaID = idEmpresaContab;

            if (AnoEmpresaContab == "" || AnoEmpresaContab == null)
            {
                logger.LogError($"Ano fiscal não foi selecionado!");
                ViewBag.Signal = "notok";
                ViewBag.ErrorTitle = "Ano fiscal não foi selecionado!";
                ViewBag.ErrorMessage = "Ano fiscal não foi selecionado!" +
                                       " Selecione ano fiscal da empresa para prosseguir com a operação!";
                return this.PartialView("~/Views/Error/GeneralError.cshtml");
            }
            string _path = "";

            IEnumerable<EmpresasViewModel> tmp_empVmodel = empresaContext.GetModelByID(EmpresaID);

            GabContabilidadeRepository gabContabilidade = new GabContabilidadeRepository(appDbContext);
            DadosEmpresaImportada dadosEmpresaImportada = gabContabilidade.GetEmpresaModel(idEmpresaContab, Int16.Parse(AnoEmpresaContab));

            if (tmp_empVmodel.ToList().Count == 1)
            {
                if (_path == "" || _path == null)
                {
                    _path = "C:\\Sage Data\\Sage Accountants\\";
                }

                EmpresasViewModel empresaViewModels = tmp_empVmodel.ToList()[0];

                _path += (_path.EndsWith("\\") ? "" : "\\") + dadosEmpresaImportada.CodeEmpresa + AnoEmpresaContab + dadosEmpresaImportada.CodeAplicacao + "\\Sync\\toCLAB";

                if (!Directory.Exists(_path))
                {
                    Directory.CreateDirectory(_path);
                }

                var claimsIdentity = (ClaimsIdentity)this.User.Identity;
                var claim = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                var userId = claim.Value;

                string nomeEmpresa = empresaViewModels.Nome;
                ViewBag.NomeEmpresa = nomeEmpresa;
                CLabViewModel tmp = new CLabViewModel
                {
                    Ano = Int16.Parse(AnoEmpresaContab),
                    EmpresaSageId = EmpresaID,
                    DataLancamento = DateTime.Now,
                    UserID = userId,
                    OutputFilePAth = _path //"C:\\Sage Data\\Sage Accountants\\" + ACtb + 2020 + ACtb + "\\Sync\\toCLAB\\OutputSageFile\\";
                };
                //tmp.OutputFilePAth = "C:\\Sage Data\\Sage Accountants\\" + ACtb + 2020 + ACtb + "\\Sync\\toCLAB\\OutputSageFile\\"  ;
                //C:\Sage Data\Sage Accountants\ACtb2020ACtb\Sync\toCLAB

                return PartialView("~/Views/CLab/Export.cshtml", tmp);
            }
            return View();
        }

        [HttpPost]
        [Route("CLab/Export")]
        public IActionResult Export( CLabViewModel model)
        {
            if (model.InputFilePathStr == null)
            {
                return Json(new  { success = false, message = "Ficheiro .asc nao foi carregado ainda!" });
            }

            EncryptionHelper encryptionHelper = new EncryptionHelper();
            string ASCFilePath = encryptionHelper.Decrypt(model.InputFilePathStr);

            int idGabContab = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "idGabContab");
            int idEmpresaContab = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "idEmpresaContab");
            string AnoEmpresaContab = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "idAnoEmpresaContab");

            //string ASCFilePath = $"{env.WebRootPath}/SageData/InputSageFiles/{model.InputFilePath.FileName}".Replace("/", "\\");

            ASCFilePath = ASCFilePath.Replace("/", "\\");

            if (!System.IO.File.Exists(ASCFilePath))
            {
                return Json(new { success = false, message = "Ficheiro .asc nao foi encontrado!" });
            }

            StreamReader streamReader = new StreamReader(ASCFilePath, Encoding.GetEncoding("iso-8859-1"));
            streamReader.DiscardBufferedData();
            streamReader.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);

            string[] ASCpathArray = ASCFilePath.Split('\\');
            string fileName = ASCpathArray[ASCpathArray.Length - 1];
            string[] fileNameArray = fileName.Split('.');
            string fileNameNoExt = fileNameArray[0];
            int index = fileNameNoExt.IndexOf(" (");
            string fileNameNoExtRep = index >= 0 ? fileNameNoExt.Remove(index) : fileNameNoExt;
            string[] fileNameNoExtArray = fileNameNoExtRep.Split('_');
            string NIF = fileNameNoExtArray[0];
            string Year = fileNameNoExtArray[fileNameNoExtArray.Length - 2];
            string Month = fileNameNoExtArray[fileNameNoExtArray.Length - 1];

            EmpresasViewModel empresaViewModel = appDbContext.EmpresasViewModel.FirstOrDefault(x => x.EmpresaID == model.EmpresaSageId);
            DadosEmpresaImportada dadosEmpresaImportada = appDbContext.DadosEmpresaImportada.FirstOrDefault(x => x.EmpresaID == model.EmpresaSageId);

            if (empresaViewModel.NIF != NIF)
            {
                return Json(new { success = false, message = "Você está tentando importar um arquivo .asc que pertence a outra empresa, verifique o NIF!" });
            }

            if (AnoEmpresaContab != Year)
            {
                return Json(new { success = false, message = "Ano fiscal do arquivo .asc e diferente do selecionado. Verifique o ano selecionado!" });
            }

            Empr empr = aGesContext.Emprs.FirstOrDefault(x => x.Ncontrib == NIF && x.Cemp == dadosEmpresaImportada.CodeEmpresa);

            DadosEmpresaImportada dadosEmpresaView = dadosEmpresaViewModel.ReturnModelByEmpresaAno(model.EmpresaSageId, Int16.Parse(Year));
            string db = dadosEmpresaView.CodeEmpresa.ToString() + Year.ToString() + dadosEmpresaView.CodeAplicacao.ToString();

            ConConfigViewModel conConfigViewModel = conConfig.FindByEmpresaId(model.EmpresaSageId).ToList()[0];

            model.strInputFilePath = ASCFilePath;
            ConverterASCtoXML conv =
                    new ConverterASCtoXML(streamReader, model, Month, db, dadosEmpresaView, conConfigViewModel, empresaViewModel, empr);


            List<ErrorLine> Errors = new List<ErrorLine>();

            conv.ConvertFile(model.OutputFilePAth);
            
            Errors = conv.Errors;
            
            if (Errors.Count>0)
            {
                return Json(new { success = true, message = "Arquivo parcialmente exportado com sucesso! Verifique os erros!", errors = JsonSerializer.Serialize(Errors)});
            }
            
            return Json(new { success = true, message = "Ficheiro convertido com sucesso!"});
        }

        [HttpPost]
        [Route("CLab/UploadFile")]
        public IActionResult UploadFile(IFormFile file)
        {
            if (file != null)
            {
                string ASCFilePath = $"{env.WebRootPath}/SageData/InputSageFiles/{file.FileName}";
                string _path = env.ContentRootPath;// WebRootFileProvider();
                string ascFl = Path.Combine($"{_path}/wwwroot/SageData/InputSageFiles/", file.FileName).Replace("/", "\\");
                ASCFilePath = ASCFilePath.Replace("/", "\\");
                if (System.IO.File.Exists(ascFl))
                {
                    System.IO.File.Delete(ascFl);
                }
                using var stream = System.IO.File.Create(@ascFl);
                file.CopyTo(stream);
                stream.Dispose();
                stream.Close();
                EncryptionHelper enc = new EncryptionHelper();
                return Json(new
                {
                    success = true
                    ,
                    message = "Carregamento do arquivo para exportação efectuado com sucesso!"
                    //,
                    //filePath = ascFl
                    , filePath= enc.Encrypt(ascFl)
                });
            }
            return Json(new { success = false, message = "Nao foi possivel carregar o arquivo para exportação!" });
        }
    }
}
