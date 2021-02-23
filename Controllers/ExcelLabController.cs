using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using toDoList.Helpers;
using toDoList.Models;
using toDoList.Models.SQL;
using toDoList.ViewModels;
using toDoList.Class;
using toDoClassLibrary47;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace toDoList.Controllers
{
    [Authorize]
    public class ExcelLabController : Controller
    {
        private int idGabContab = 0; 
        private int idEmpresaContab = 0;  
        private string AnoEmpresaContab = "";
        private readonly IWebHostEnvironment env;

        private readonly ILogger<AdministrationController> logger;
        private readonly AGesContext aGesContext;
        private readonly IEmpresa empresaContext;
        private readonly AppDbContext appDbContext;
        private readonly IConConfig conConfig;

        EncryptionHelper encryptionHelper = new EncryptionHelper();

        public ExcelLabController(ILogger<AdministrationController> _logger, AGesContext _aGesContext, IWebHostEnvironment _env, IEmpresa _empresaContext, AppDbContext _appDbContext, IConConfig _conConfig)
        {
            this.logger = _logger;
            this.aGesContext = _aGesContext;
            this.empresaContext = _empresaContext;
            this.appDbContext = _appDbContext;
            this.conConfig = _conConfig;
            this.env = _env;
        }

        public IActionResult Index()
        {
            idGabContab = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "sessionIDGabContab");
            idEmpresaContab = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "sessionIDEmpresaContab");
            AnoEmpresaContab = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "sessionIDAnoEmpresaContab");

            int EmpresaID;
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


            if (AnoEmpresaContab == "" || AnoEmpresaContab == null)
            {
                logger.LogError($"Ano fiscal não foi selecionado!");
                ViewBag.Signal = "notok";
                ViewBag.ErrorTitle = "Ano fiscal não foi selecionado!";
                ViewBag.ErrorMessage = "Ano fiscal não foi selecionado!" +
                                       " Selecione ano fiscal da empresa para prosseguir com a operação!";
                return this.PartialView("~/Views/Error/GeneralError.cshtml");
            }

            EmpresaID = idEmpresaContab;

            IEnumerable<EmpresasViewModel> tmp_empVmodel = empresaContext.GetModelByID(idEmpresaContab);
            GabContabilidadeRepository gabContabilidade = new GabContabilidadeRepository(appDbContext);
            DadosEmpresaImportada dadosEmpresaImportada = gabContabilidade.GetEmpresaModel(idEmpresaContab, Int16.Parse(AnoEmpresaContab));

            string baseDados = dadosEmpresaImportada.CodeEmpresa + AnoEmpresaContab + dadosEmpresaImportada.CodeAplicacao;

            string connString = "server=.;database=" + baseDados + ";Trusted_Connection=true;MultipleActiveResultSets=true;";

            if (AnoEmpresaContab == "" || AnoEmpresaContab == null)
            {
                logger.LogError($"Ano fiscal não foi selecionado!");
                ViewBag.Signal = "notok";
                ViewBag.ErrorTitle = "Ano fiscal não foi selecionado!";
                ViewBag.ErrorMessage = "Ano fiscal não foi selecionado!" +
                                       " Selecione ano fiscal da empresa para prosseguir com a operação!";
                return this.PartialView("~/Views/Error/GeneralError.cshtml");
            }
            ExcelLabViewModel tmp = new ExcelLabViewModel();
            tmp.CodEmpresa = dadosEmpresaImportada.CodeEmpresa;
            tmp.DataLancamento = DateTime.Now;
            tmp.DiarioLancamento = GetDiariosLancamento(connString);
            tmp.AnoLancamento = Int16.Parse(AnoEmpresaContab);
            tmp.MesLancamento = toDoClassLibrary.MonthEnum.Janeiro;
            tmp.TipoLancamento = GetTipoLancamento(connString);
            tmp.TipoDocumento = GetTipoDocumento(connString);
            tmp.LancamentoUnico = false;
            tmp.InputFilePath = "";
            tmp.OutputFilePath = "C:\\Sage Data\\Sage Accountants\\" + dadosEmpresaImportada.CodeEmpresa + AnoEmpresaContab + dadosEmpresaImportada.CodeAplicacao ; 

            return PartialView("~/Views/ExcelLab/Index.cshtml", tmp);
   
        }
        private List<SelectListItem> GetTipoDocumento(string connString)
        {
            using var context = new CustomDbContext(connString);
            Sql_IDiarioLancamento iDiario = new Sql_IDiarioLancamento(context);
            SelectListItem item_Zero = new SelectListItem()
            {
                Value = "0",
                Text = "-- Selecione Tp. Documento --"
            };
            List<SelectListItem> selectListItems = iDiario.GetTipoDocumento();
            selectListItems.Insert(0, item_Zero);

            return selectListItems;//Json(new { success = true, list = selectListItems });
        }
        private List<SelectListItem> GetTipoLancamento(string connString)
        {
            using var context = new CustomDbContext(connString);
            Sql_IDiarioLancamento iDiario = new Sql_IDiarioLancamento(context);
            SelectListItem item_Zero = new SelectListItem()
            {
                Value = "0",
                Text = "-- Selecione Tp. Lancamento --"
            };
            List<SelectListItem> selectListItems = iDiario.GetTipoLancamento();
            selectListItems.Insert(0, item_Zero);
            return selectListItems;
        }
        private List<SelectListItem> GetDiariosLancamento(string connString)
        {
            using var context = new CustomDbContext(connString);
            Sql_IDiarioLancamento iDiario = new Sql_IDiarioLancamento(context);
 
            SelectListItem item_Zero = new SelectListItem()
            {
                Value = "0",
                Text = "-- Selecione Diar. Lancamento --"
            };
 
            List<SelectListItem> selectListItems = iDiario.GetDiarioLancemento  ();
            selectListItems.Insert(0, item_Zero);

            return selectListItems;//Json(new { success = true, list = selectListItems });
        }

        [HttpGet]
        public JsonResult FillDiarioLancamento()
        {
            int idGabContab = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "sessionIDGabContab");
            int idEmpresaContab = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "sessionIDEmpresaContab");
            string AnoEmpresaContab = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "sessionIDAnoEmpresaContab");

            IEnumerable<EmpresasViewModel> tmp_empVmodel = empresaContext.GetModelByID(idEmpresaContab);

            GabContabilidadeRepository gabContabilidade = new GabContabilidadeRepository(appDbContext);
            DadosEmpresaImportada dadosEmpresaImportada = gabContabilidade.GetEmpresaModel(idEmpresaContab, Int16.Parse(AnoEmpresaContab));

            string baseDados = dadosEmpresaImportada.CodeEmpresa + AnoEmpresaContab + dadosEmpresaImportada.CodeAplicacao;

            string connString = "server=.;database=" + baseDados + ";Trusted_Connection=true;MultipleActiveResultSets=true;";
            using var context = new CustomDbContext(connString);

            Sql_IDiarioLancamento iDiario = new Sql_IDiarioLancamento(context);

            List<SelectListItem> selectListItems = iDiario.GetDiarioLancemento();

            return Json(new { success = true, list = selectListItems });
        }

        [HttpGet]
        [Route("ExcelLab/GetExample")]
        public  async Task<IActionResult> GetExampleAsync() 
        {
            string remoteUri = $"{env.WebRootPath}/Exemplos Excel/";
            string fileName = "NIFempresaSage_Ano_MesNumero.xlsx";
            string excelFileFullPath = Path.Combine($"{remoteUri}", fileName).Replace("/", "\\");

            int idEmpresaContab = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "sessionIDEmpresaContab");
            string AnoEmpresaContab = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "sessionIDAnoEmpresaContab");

            IEnumerable<EmpresasViewModel> tmp_empVmodel = empresaContext.GetModelByID(idEmpresaContab);
            GabContabilidadeRepository gabContabilidade = new GabContabilidadeRepository(appDbContext);
            DadosEmpresaImportada dadosEmpresaImportada = gabContabilidade.GetEmpresaModel(idEmpresaContab, Int16.Parse(AnoEmpresaContab));

            var memory = new MemoryStream();
            using (var stream = new FileStream(excelFileFullPath,FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var ext = Path.GetExtension(excelFileFullPath).ToLowerInvariant();
            var tExt = GetMimeTypes()[ext];
            int _year = DateTime.Now.Year;
            int _month = DateTime.Now.Month;
            string outputFileName = $"{tmp_empVmodel.ToList()[0].NIF}_{dadosEmpresaImportada.CodeEmpresa}{dadosEmpresaImportada.CodeAplicacao}_{_year.ToString()}_{_month.ToString()}.xlsx";
            
            return File(memory, GetMimeTypes()[ext], outputFileName);
        }
        private Dictionary<string, string> GetMimeTypes() 
        {
            return new Dictionary<string, string>
            {
                {".txt","text/plain" },
                {".pdf","application/pdf" },
                {".doc","application/vnd.mx-word" },
                {".docx","application/vnd.mx-word" },
                {".xls","application/vnd.mx-excel" },
                {".xlsx","application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
                {".png","image/png" },
                {".jpg","image/jpeg" },
                {".jpeg","image/jpeg" },
                {".gif","image/gif" },
                {".csv","text/csv" }
            };
        }

        [HttpGet]
        [Route("ExcelLab/ConvertFile")]
        public JsonResult ConvertFile(ExcelLabViewModel model)
        {
            if (model.DiarioLancamentoInt == "0")
            {
                return Json(new { success = false, message = "Escolha um diário de lançamento!" });
            }

            if (model.TipoLancamentoInt == "0")
            {
                return Json(new { success = false, message = "Escolha o tipo de lançamento!" });
            }

            if (model.TipoDocumentoInt == "0")
            {
                return Json(new { success = false, message = "Escolha o tipo de documento!" });
            }
            if (model.InputFilePath == null)
            {
                return Json(new { success = false, message = "Escolha um ficheiro *.xlsx para importar" });
            }

            int idGabContab = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "sessionIDGabContab");
            int idEmpresaContab = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "sessionIDEmpresaContab");
            string AnoEmpresaContab = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "sessionIDAnoEmpresaContab");

            IEnumerable<EmpresasViewModel> tmp_empVmodel = empresaContext.GetModelByID(idEmpresaContab);
            GabContabilidadeRepository gabContabilidade = new GabContabilidadeRepository(appDbContext);
            DadosEmpresaImportada dadosEmpresaImportada = gabContabilidade.GetEmpresaModel(idEmpresaContab, Int16.Parse(AnoEmpresaContab));

            string baseDados = dadosEmpresaImportada.CodeEmpresa + AnoEmpresaContab + dadosEmpresaImportada.CodeAplicacao;

            string connString = "server=.;database=" + baseDados + ";Trusted_Connection=true;MultipleActiveResultSets=true;";

            using var context = new CustomDbContext(connString);

            string importedExcelFilePath = encryptionHelper.Decrypt(model.InputFilePath);

            model.InputFilePath = importedExcelFilePath;

            //FileUpload1.SaveAs(ExcelFilePath);

            string XMLFilePath = model.OutputFilePath;

            //string LocalCon = Session["LocalConnection"].ToString();

            string CodEmpSage = dadosEmpresaImportada.CodeEmpresa;

            //string EmpSageCon = Session["EmpSageConnection"].ToString();

            string CodDiario = model.DiarioLancamentoInt; //this.ddrDiarioLanc.SelectedValue;
            string TipoLanc = model.TipoLancamentoInt;//this.ddrTipoLanc.SelectedValue;
            string TipoDoc = model.TipoDocumentoInt; //this.ddrTipoDoc.SelectedValue;
            bool LancUnico = model.LancamentoUnico;

            string[] ExcelPathArray = importedExcelFilePath.Split('\\');
            string fileName = ExcelPathArray[ExcelPathArray.Length - 1];
            string[] fileNameArray = fileName.Split('.');
            string fileNameNoExt = fileNameArray[0];
            int index = fileNameNoExt.IndexOf(" (");
            string fileNameNoExtRep = index >= 0 ? fileNameNoExt.Remove(index) : fileNameNoExt;
            string[] fileNameNoExtArray = fileNameNoExtRep.Split('_');
            string NIF = fileNameNoExtArray[0];
            string Year = fileNameNoExtArray[1];

            //using (SqlConnection con = new SqlConnection(Session["LocalConnection"].ToString()))
            //{
            //    con.Open();

            //    string selectEmpresaSage_query = "select NContrib from Emprs where CEmp = '" + Session["EmpSage"].ToString() + "'";
            //    SqlCommand selectEmpresaSage = new SqlCommand(selectEmpresaSage_query, con);
            //    string NifEmpSage = selectEmpresaSage.ExecuteScalar().ToString();

            if (NIF != tmp_empVmodel.ToList()[0].NIF)
            {
                return Json(new { success = false, message = "Contribuinte nao coincide, verifique o contribuinte a importar!" });
            }
            if (Year != AnoEmpresaContab)
            {
                return Json(new { success = false, message = "O Ano seleccionado nao coresponde com o Ano do ficheiro a importar!" });
            }


            string DataLanc = model.DataLancamento.ToString("yyyy-MM-dd");//Convert.ToDateTime(this.txtDataLanc.Text).ToString("yyyy-MM-dd");

            ConConfigViewModel conConfigViewModel = conConfig.FindByEmpresaId(idEmpresaContab).ToList()[0];
            EmpresasViewModel empresaViewModel = appDbContext.EmpresasViewModel.FirstOrDefault(x => x.EmpresaID == idEmpresaContab);
            Empr empr = aGesContext.Emprs.FirstOrDefault(x => x.Ncontrib == NIF && x.Cemp == dadosEmpresaImportada.CodeEmpresa);

            var conv = new ConvertExcelToXML(model, baseDados, dadosEmpresaImportada, conConfigViewModel, empresaViewModel, empr);

            List<ErrorLine> Errors = new List<ErrorLine>();

            conv.ConvertFile();
            Errors = conv.errors;

            if (Errors.Count > 0)
            {
                return Json(new { success = true, message = "Arquivo parcialmente exportado com sucesso! Verifique os erros!", errors = JsonSerializer.Serialize(Errors) });
            }

            return Json(new { success = true, message = "Ficheiro convertido com sucesso!" });

            //    var memory = new MemoryStream();
            //using (var stream = new FileStream(importedExcelFilePath, FileMode.Open))
            //{
            //    await stream.CopyToAsync(memory);
            //}
            //memory.Position = 0;
            //var ext = Path.GetExtension(importedExcelFilePath).ToLowerInvariant();
            //var tExt = GetMimeTypes()[ext];
            //int _year = DateTime.Now.Year;
            //int _month = DateTime.Now.Month;
            //string outputFileName = $"{tmp_empVmodel.ToList()[0].NIF}_{dadosEmpresaImportada.CodeEmpresa}{dadosEmpresaImportada.CodeAplicacao}_{_year.ToString()}_{_month.ToString()}.xlsx";


        }

    }
}
