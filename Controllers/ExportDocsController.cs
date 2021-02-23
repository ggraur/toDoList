
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using toDoList.Helpers;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Microsoft.Extensions.Logging;
using toDoList.Models;
using toDoList.ViewModels;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using toDoClassLibrary47;
using System.Data;

using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Microsoft.Extensions.Hosting.Internal;
using System.Net.Http.Headers;

namespace toDoList.Controllers
{
    [Authorize]
    public class ExportDocsController : Controller
    {
        private readonly ILogger<AdministrationController> logger;
        private readonly IConConfig conConfig;
        private readonly IEmpresaAGes empresaAGes;
        private readonly IDadosEmpresaViewModel dadosEmpresaViewModel;
 
        public ExportDocsController(ILogger<AdministrationController> _logger,
            IConConfig _conConfig,
            IEmpresaAGes _empresaAGes,
            IDadosEmpresaViewModel _dadosEmpresaViewModel )
        {
            this.logger = _logger;
            this.conConfig = _conConfig;
            this.empresaAGes = _empresaAGes;
            this.dadosEmpresaViewModel = _dadosEmpresaViewModel;
            
        }




        //[HttpPost]
        //public async Task<IActionResult> UploadFilesAjax(IEnumerable<IFormFile> files)
        //{
        //    HostingEnvironment hosting = new HostingEnvironment();
        //    var uploads = Path.Combine(hosting.ContentRootPath, "images");
        //    foreach (var file in files)
        //    {
        //        //var filename = ContentDispositionHeaderValue
        //        //        .Parse(file.ContentDisposition)
        //        //        .FileName
        //        //        .Trim('"');

        //        if (file != null && file.Length > 0)
        //        {
        //            var fileName = Guid.NewGuid().ToString().Replace("-", "") +
        //                            Path.GetExtension(file.FileName);
        //            using var s = new FileStream(Path.Combine(uploads, fileName), FileMode.Create);
        //            await file.CopyToAsync(s);
        //            // model.imgName = fileName;
        //        }
        //    }
        //    return Json(new { status = "success", message = "success" });

        //}

        [HttpGet]
        public IActionResult Index(string _EmpSage, string _Ano)
        {
            //PlanoContasViewModelPath planoContasView
            int idGabContab = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "sessionIDGabContab");
            int idEmpresaContab = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "sessionIDEmpresaContab");
            string AnoEmpresaContab = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "sessionIDAnoEmpresaContab");

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

            List<ConConfigViewModel> conConfigViewModel = conConfig.FindByEmpresaId(idEmpresaContab).ToList();

            if (conConfigViewModel.Count == 0)
            {
                GeneralErrorViewModel generalErrorViewModel = new GeneralErrorViewModel
                {
                    Signal = "notok",
                    ErrorTitle = "Conexão nao configurada!",
                    ErrorMessage = "A empresa não tem uma conexão configurada!",
                    StringButton = "Configurar conexão",
                    UrlToRedirect = "/ConConfig/IndexJson/",
                    optionalData = idEmpresaContab.ToString()
                };
                return this.PartialView("~/Views/Error/GeneralErrorModel.cshtml", generalErrorViewModel);

                //ViewBag.Signal = "notok";
                //ViewBag.ErrorTitle = "Conexão nao configurada!";
                //ViewBag.ErrorMessage = "A empresa não tem uma conexão configurada!";
                //return this.PartialView("~/Views/Error/GeneralError.cshtml");
            }

            DadosEmpresaImportada empViewModel = dadosEmpresaViewModel.ReturnModelByEmpresaAno(idEmpresaContab, Int16.Parse(AnoEmpresaContab));

            string _servidorSQL = conConfigViewModel[0].NomeServidor;
            string _instanciaSQL = conConfigViewModel[0].InstanciaSQL;

            string _path = "";

            /*
                       AGesLocEn  _AGesLocEn = empresaAGes.GetAGesLocEn();
                      string _path = _AGesLocEn.GesSharedDir + _servidorSQL + "\\" + _instanciaSQL;

              */

            if (_path == "" || _path == null)
            {
                _path = "C:\\Sage Data\\Sage Accountants\\";
            }

            _path += (_path.EndsWith("\\") ? "" : "\\") + empViewModel.CodeEmpresa + _Ano + empViewModel.CodeAplicacao + "\\Sync\\toCLAB";

            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }

            PathViewModel model = new PathViewModel
            {
                Path = _path,
                EmpresaID = idEmpresaContab,
                Ano = Int16.Parse(AnoEmpresaContab)
            };
            return this.PartialView("~/Views/ExportDocs/Index.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ExportDocs/ExportPlContasAsync")]
        public async Task<IActionResult> ExportPlContasAsync(string EmpresaID, string Ano, string _path)
        {
            DadosEmpresaImportada dadosEmpresaView = dadosEmpresaViewModel.ReturnModelByEmpresaAno(int.Parse(EmpresaID), Int16.Parse(Ano));
            string db = dadosEmpresaView.CodeEmpresa.ToString() + Ano.ToString() + dadosEmpresaView.CodeAplicacao.ToString();
            string _nomeFicheiro = DateTime.Now.ToString("yyyyMMdd_hhmmss") + "_" + dadosEmpresaView.CodeAplicacao.ToString() + Ano + "_CLAB_PlanodeContas.xlsx";

            ConConfigViewModel conConfigViewModel = conConfig.FindByEmpresaId(int.Parse(EmpresaID)).ToList()[0];
            clsCheckValidData checkValidData = new clsCheckValidData(conConfigViewModel.NomeServidor, conConfigViewModel.Utilizador, conConfigViewModel.Password, db);

            bool validDbAndServer = checkValidData.ValidDbAndServer();
            if (validDbAndServer != true)
            {
                ViewBag.Signal = "notok";
                ViewBag.ErrorTitle = "Base de dados ou servidor inexistente!";
                ViewBag.ErrorMessage = $"Por favour verifique a existencia da Base de dados: {dadosEmpresaView.CodeEmpresa + dadosEmpresaView.CodeAplicacao + dadosEmpresaView.AnoFi.ToString()} !";
                return this.PartialView("~/Views/Error/GeneralError.cshtml");
            }

            string strSql = "SELECT CConta as 'Nº da Conta', CASE WHEN CContaMae = '' THEN 'R' WHEN CContaMae != '' AND EdeMov = 0 THEN 'I' WHEN EdeMov = 1 THEN 'L' ELSE '' END as 'Tp.', Descr as 'Nome da conta', CCIva as 'Conta IVA/Ref' FROM Conta ORDER BY CConta";
            DataTable dt = checkValidData.ReturnDataTable(strSql);

            string _FullPath = _path + "\\" + _nomeFicheiro;

            await SavePlContasAsync(_path, _nomeFicheiro, dt);
            //   bool aa= SavePlContasAsync(_path, _nomeFicheiro, dt); ;
            if (!System.IO.File.Exists(_FullPath))
            {
                return Json(new { success = false, msg = "O arquivo nao foi exportado com sucesso!" });
            }
            return Json(new { success = true, msg = "Arquivo exportado com sucesso!" });
        }

        private async Task SavePlContasAsync(string _path, string _fileName, DataTable _dt)
        {
            //https://dzone.com/articles/import-and-export-excel-file-in-asp-net-core-31-ra
            //https://stackoverflow.com/questions/48259072/how-to-call-async-task-from-ajax-aspnet-core
            {

                string _finalFileName = $"{_path}\\{_fileName}";

                FileInfo file = new FileInfo(Path.Combine(_path, _fileName));

                var memory = new MemoryStream();

                using (var fs = new FileStream(Path.Combine(_path, _fileName), FileMode.Create, FileAccess.Write))
                {
                    var currentRow = 0;
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("PlanoContas");
                    IRow row = excelSheet.CreateRow(currentRow);
                    row.CreateCell(0).SetCellValue("Nº da Conta");
                    row.CreateCell(1).SetCellValue("Tp.");
                    row.CreateCell(2).SetCellValue("Nome da Conta");
                    row.CreateCell(3).SetCellValue("Conta IVA/Ref");
                    foreach (DataRow dr in _dt.Rows)
                    {
                        currentRow++;
                        row = excelSheet.CreateRow(currentRow);
                        row.CreateCell(0).SetCellValue(dr[0].ToString());
                        row.CreateCell(1).SetCellValue(dr[1].ToString());
                        row.CreateCell(2).SetCellValue(dr[2].ToString());
                        row.CreateCell(3).SetCellValue(dr[3].ToString());
                    }
                    workbook.Write(fs);
                }
                using (var stream = new FileStream(Path.Combine(_path, _fileName), FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                    //stream.CopyTo(memory);

                }
                memory.Position = 0;

                File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Path.Combine(_path, _fileName));
                // return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
                //return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", _fileName);

            }
        }


        private async Task SaveTpDocsAsync(string _path, string _fileName, DataTable _dt)
        {
            //https://dzone.com/articles/import-and-export-excel-file-in-asp-net-core-31-ra
            //https://stackoverflow.com/questions/48259072/how-to-call-async-task-from-ajax-aspnet-core
            {
                string _finalFileName = $"{_path}\\{_fileName}";

                FileInfo file = new FileInfo(Path.Combine(_path, _fileName));

                var memory = new MemoryStream();

                using (var fs = new FileStream(Path.Combine(_path, _fileName), FileMode.Create, FileAccess.Write))
                {
                    var currentRow = 0;
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("PlanoContas");
                    IRow row = excelSheet.CreateRow(currentRow);
                    row.CreateCell(0).SetCellValue("Documento");
                    row.CreateCell(1).SetCellValue("Descrição");
                    foreach (DataRow dr in _dt.Rows)
                    {
                        currentRow++;
                        row = excelSheet.CreateRow(currentRow);
                        row.CreateCell(0).SetCellValue(dr[0].ToString());
                        row.CreateCell(1).SetCellValue(dr[1].ToString());
                    }
                    workbook.Write(fs);
                }
                using (var stream = new FileStream(Path.Combine(_path, _fileName), FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;

                File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Path.Combine(_path, _fileName));

                // return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
                //return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", _fileName);
            }
        }


        private async Task SaveCentroCustoAsync(string _path, string _fileName, DataTable _dt)
        {
            //https://dzone.com/articles/import-and-export-excel-file-in-asp-net-core-31-ra
            //https://stackoverflow.com/questions/48259072/how-to-call-async-task-from-ajax-aspnet-core
            {
                string _finalFileName = $"{_path}\\{_fileName}";

                FileInfo file = new FileInfo(Path.Combine(_path, _fileName));

                var memory = new MemoryStream();

                using (var fs = new FileStream(Path.Combine(_path, _fileName), FileMode.Create, FileAccess.Write))
                {
                    var currentRow = 0;
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("PlanoContas");
                    IRow row = excelSheet.CreateRow(currentRow);
                    row.CreateCell(0).SetCellValue("Centro Custo");
                    row.CreateCell(1).SetCellValue("Descrição");
                    foreach (DataRow dr in _dt.Rows)
                    {
                        currentRow++;
                        row = excelSheet.CreateRow(currentRow);
                        row.CreateCell(0).SetCellValue(dr[0].ToString());
                        row.CreateCell(1).SetCellValue(dr[1].ToString());
                    }
                    workbook.Write(fs);
                }
                using (var stream = new FileStream(Path.Combine(_path, _fileName), FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;

                File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Path.Combine(_path, _fileName));

                // return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
                //return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", _fileName);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ExportDocs/ExportTpDocsAsync")]
        public async Task<IActionResult> ExportTpDocsAsync(string EmpresaID, string Ano, string _path)
        {
            DadosEmpresaImportada dadosEmpresaView = dadosEmpresaViewModel.ReturnModelByEmpresaAno(int.Parse(EmpresaID), Int16.Parse(Ano));
            string db = dadosEmpresaView.CodeEmpresa.ToString() + Ano.ToString() + dadosEmpresaView.CodeAplicacao.ToString();
            string _nomeFicheiro = DateTime.Now.ToString("yyyyMMdd_hhmmss") + "_" + dadosEmpresaView.CodeAplicacao.ToString() + Ano + "_CLAB_Documentos.xlsx";

            ConConfigViewModel conConfigViewModel = conConfig.FindByEmpresaId(int.Parse(EmpresaID)).ToList()[0];
            clsCheckValidData checkValidData = new clsCheckValidData(conConfigViewModel.NomeServidor, conConfigViewModel.Utilizador, conConfigViewModel.Password, db);

            bool validDbAndServer = checkValidData.ValidDbAndServer();
            if (validDbAndServer != true)
            {
                ViewBag.Signal = "notok";
                ViewBag.ErrorTitle = "Base de dados ou servidor inexistente!";
                ViewBag.ErrorMessage = $"Por favour verifique a existencia da Base de dados: {dadosEmpresaView.CodeEmpresa + dadosEmpresaView.CodeAplicacao + dadosEmpresaView.AnoFi.ToString()} !";
                return this.PartialView("~/Views/Error/GeneralError.cshtml");
            }

            string strSql = "SELECT Sigla as 'Documento', Descr as 'Descrição' FROM TpDoc ORDER BY TDoc";

            DataTable dt = checkValidData.ReturnDataTable(strSql);

            string _FullPath = _path + "\\" + _nomeFicheiro;

            await SaveTpDocsAsync(_path, _nomeFicheiro, dt);

            if (!System.IO.File.Exists(_FullPath))
            {
                return Json(new { success = false, msg = "O arquivo nao foi exportado com sucesso!" });
            }
            return Json(new { success = true, msg = "Arquivo exportado com sucesso!" });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ExportDocs/ExportSaveCentroCustoAsyncAsync")]
        public async Task<IActionResult> ExportSaveCentroCustoAsyncAsync(string EmpresaID, string Ano, string _path)
        {
            DadosEmpresaImportada dadosEmpresaView = dadosEmpresaViewModel.ReturnModelByEmpresaAno(int.Parse(EmpresaID), Int16.Parse(Ano));
            string db = dadosEmpresaView.CodeEmpresa.ToString() + Ano.ToString() + dadosEmpresaView.CodeAplicacao.ToString();
            string _nomeFicheiro = DateTime.Now.ToString("yyyyMMdd_hhmmss") + "_" + dadosEmpresaView.CodeAplicacao.ToString() + Ano + "_CLAB_CentrosCusto.xlsx";

            ConConfigViewModel conConfigViewModel = conConfig.FindByEmpresaId(int.Parse(EmpresaID)).ToList()[0];
            clsCheckValidData checkValidData = new clsCheckValidData(conConfigViewModel.NomeServidor, conConfigViewModel.Utilizador, conConfigViewModel.Password, db);

            bool validDbAndServer = checkValidData.ValidDbAndServer();
            if (validDbAndServer != true)
            {
                ViewBag.Signal = "notok";
                ViewBag.ErrorTitle = "Base de dados ou servidor inexistente!";
                ViewBag.ErrorMessage = $"Por favour verifique a existencia da Base de dados: {dadosEmpresaView.CodeEmpresa + dadosEmpresaView.CodeAplicacao + dadosEmpresaView.AnoFi.ToString()} !";
                return this.PartialView("~/Views/Error/GeneralError.cshtml");
            }

            string strSql = "SELECT CCeCu as 'Centro Custo', Descr as 'Descrição' FROM CenCu ORDER BY TCeCu, CCeCu";

            DataTable dt = checkValidData.ReturnDataTable(strSql);

            string _FullPath = _path + "\\" + _nomeFicheiro;

            await SaveCentroCustoAsync(_path, _nomeFicheiro, dt);

            if (!System.IO.File.Exists(_FullPath))
            {
                return Json(new { success = false, msg = "O arquivo nao foi exportado com sucesso!" });
            }
            return Json(new { success = true, msg = "Arquivo exportado com sucesso!" });
        }
    }
}
