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
namespace toDoList.Controllers
{
    public class ExcelLabController : Controller
    {
        private int idGabContab = 0; 
        private int idEmpresaContab = 0;  
        private string AnoEmpresaContab = "";
        private readonly IWebHostEnvironment env;

        private readonly ILogger<AdministrationController> logger;
        private readonly IEmpresa empresaContext;
        private readonly AppDbContext appDbContext;

        public ExcelLabController(ILogger<AdministrationController> _logger, IWebHostEnvironment _env, IEmpresa _empresaContext, AppDbContext _appDbContext)
        {
            this.logger = _logger;
            this.empresaContext = _empresaContext;
            this.appDbContext = _appDbContext;
            this.env = _env;
        }

        public IActionResult Index()
        {
            idGabContab = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "idGabContab");
            idEmpresaContab = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "idEmpresaContab");
            AnoEmpresaContab = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "idAnoEmpresaContab");

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
            tmp.OutputFilePath = "";

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
            int idGabContab = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "idGabContab");
            int idEmpresaContab = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "idEmpresaContab");
            string AnoEmpresaContab = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "idAnoEmpresaContab");

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

            int idEmpresaContab = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "idEmpresaContab");
            string AnoEmpresaContab = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "idAnoEmpresaContab");

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
            string outputFileName = $"{tmp_empVmodel.ToList()[0].NIF}{dadosEmpresaImportada.CodeEmpresa}{dadosEmpresaImportada.CodeAplicacao}_{_year.ToString()}_{_month.ToString()}.xlsx";
            
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
        public async Task<JsonResult> ConvertFile(ExcelLabViewModel model)
        {

            IEnumerable<EmpresasViewModel> tmp_empVmodel = empresaContext.GetModelByID(idEmpresaContab);
            GabContabilidadeRepository gabContabilidade = new GabContabilidadeRepository(appDbContext);
            DadosEmpresaImportada dadosEmpresaImportada = gabContabilidade.GetEmpresaModel(idEmpresaContab, Int16.Parse(AnoEmpresaContab));

            string baseDados = dadosEmpresaImportada.CodeEmpresa + AnoEmpresaContab + dadosEmpresaImportada.CodeAplicacao;

            string connString = "server=.;database=" + baseDados + ";Trusted_Connection=true;MultipleActiveResultSets=true;";
            
            using var context = new CustomDbContext(connString);

            string importedExcelFilePath = model.InputFilePath;

            //FileUpload1.SaveAs(ExcelFilePath);

            //string XMLFilePath = this.txtOutput.Text;

            //string LocalCon = Session["LocalConnection"].ToString();
            //string CodEmpSage = Session["EmpSage"].ToString();
            //string EmpSageCon = Session["EmpSageConnection"].ToString();

            //string CodDiario = this.ddrDiarioLanc.SelectedValue;
            //string TipoLanc = this.ddrTipoLanc.SelectedValue;
            //string TipoDoc = this.ddrTipoDoc.SelectedValue;
            //bool LancUnico = this.chkLancUnico.Checked;

            //string[] ExcelPathArray = ExcelFilePath.Split('\\');
            //string fileName = ExcelPathArray[ExcelPathArray.Length - 1];
            //string[] fileNameArray = fileName.Split('.');
            //string fileNameNoExt = fileNameArray[0];
            //int index = fileNameNoExt.IndexOf(" (");
            //string fileNameNoExtRep = index >= 0 ? fileNameNoExt.Remove(index) : fileNameNoExt;
            //string[] fileNameNoExtArray = fileNameNoExtRep.Split('_');
            //string NIF = fileNameNoExtArray[0];
            //string Year = fileNameNoExtArray[1];

            //using (SqlConnection con = new SqlConnection(Session["LocalConnection"].ToString()))
            //{
            //    con.Open();

            //    string selectEmpresaSage_query = "select NContrib from Emprs where CEmp = '" + Session["EmpSage"].ToString() + "'";
            //    SqlCommand selectEmpresaSage = new SqlCommand(selectEmpresaSage_query, con);
            //    string NifEmpSage = selectEmpresaSage.ExecuteScalar().ToString();

            //    if (NIF != NifEmpSage && Year != Session["Ano"].ToString())
            //    {
            //        ShowMessage("O NIF e ano presentes no nome do ficheiro Excel não correspondem ao NIF e ano da empresa selecionada.", false, false, false, true);
            //        return;
            //    }
            //    else if (NIF != NifEmpSage)
            //    {
            //        ShowMessage("O NIF presente no nome do ficheiro Excel não corresponde ao NIF da empresa selecionada.", false, false, false, true);
            //        return;
            //    }

            //    con.Close();
            //    con.Dispose();
            //}

            //if (Year != Session["Ano"].ToString())
            //{
            //    ShowMessage("O ano presente no nome do ficheiro Excel não corresponde ao ano selecionado.", false, false, false, true);
            //    return;
            //}

            //string DataLanc = Convert.ToDateTime(this.txtDataLanc.Text).ToString("yyyy-MM-dd");

            //var conv = new ConvertExcelToXML( DataLanc, ExcelFilePath, XMLFilePath, LocalCon, CodEmpSage, EmpSageCon, CodDiario, TipoLanc, TipoDoc, LancUnico);
            //conv.ConvertFile();

            var memory = new MemoryStream();
            using (var stream = new FileStream(importedExcelFilePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var ext = Path.GetExtension(importedExcelFilePath).ToLowerInvariant();
            var tExt = GetMimeTypes()[ext];
            int _year = DateTime.Now.Year;
            int _month = DateTime.Now.Month;
            string outputFileName = $"{tmp_empVmodel.ToList()[0].NIF}{dadosEmpresaImportada.CodeEmpresa}{dadosEmpresaImportada.CodeAplicacao}_{_year.ToString()}_{_month.ToString()}.xlsx";

            return Json(new { success = true });
        }

    }
}
