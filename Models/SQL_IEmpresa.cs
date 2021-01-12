using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoList.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data.Common;
using System;

namespace toDoList.Models
{
    public class SQL_IEmpresa : IEmpresa
    {
        private readonly AppDbContext context;
        private readonly ILogger<SQL_IEmpresa> logger;
        private readonly IDadosEmpresaViewModel dadosEmpresaView;
        private readonly AGesContext aGesContext;

        public SQL_IEmpresa(AppDbContext _context,
                    ILogger<SQL_IEmpresa> _logger,
                IDadosEmpresaViewModel _dadosEmpresaView,
                AGesContext _aGesContext)
        {
            this.context = _context;
            this.logger = _logger;
            this.dadosEmpresaView = _dadosEmpresaView;
            this.aGesContext = _aGesContext;
        }

        public int ReturnCompanyID(string nomeEmpresa, string NIF)
        {
            int iResult = -1;
            try
            {
                int tmp = context.EmpresasViewModel
                                  .Where(x => x.Nome == nomeEmpresa && x.NIF == NIF)
                                  .Distinct()
                                  .Select(x => x.EmpresaID)
                                  .ToList<int>()[0];
                if (tmp != 0)
                {
                    iResult = tmp;
                }
                return iResult;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message);
                return iResult;
            }

        }
        public async Task<bool> RemoveUtilizadorFromEmpresaAsync(EmpresaUtilizadoresViewModel tmpUser)
        {
            try
            {
                EmpresaUtilizadoresViewModel tmp = (EmpresaUtilizadoresViewModel)context.EmpresaUtilizadores.Where(x => x.UserID == tmpUser.UserID);
                using (context)
                {
                    context.EmpresaUtilizadores.RemoveRange(tmp);
                    int iCount = await context.SaveChangesAsync();
                    return (iCount > 0) ? true : false;
                }
            }
            catch (DbUpdateException ex)
            {
                logger.LogCritical(ex.Message);
                return false;
            }

        }

        public async Task<bool> RemoveFromUtilizadoresEmpresaAsync(int EmpresaID)
        {
            try
            {
                IEnumerable<EmpresaUtilizadoresViewModel> tmp = (IEnumerable<EmpresaUtilizadoresViewModel>)context.EmpresaUtilizadores.Where(x => x.EmpresaId == EmpresaID);
                using (context)
                {
                    context.EmpresaUtilizadores.RemoveRange(tmp);
                    int iCount = await context.SaveChangesAsync();
                    return (iCount > 0) ? true : false;
                }
            }
            catch (DbUpdateException ex)
            {
                logger.LogCritical(ex.Message);
                return false;
            }
        }

        public async Task<bool> AddUtilizadorEmpresaAsync(int EmpresaID, EmpresaUtilizadoresViewModel user)
        {
            try
            {
                using (context)
                {
                    user.EmpresaId = EmpresaID;
                    context.EmpresaUtilizadores.Add(user);
                    int iCount = await context.SaveChangesAsync();
                    return (iCount > 0) ? true : false;
                }
            }
            catch (DbUpdateException ex)
            {
                logger.LogCritical(ex.Message);
                return false;
            }
        }
        public async Task<bool> AddUtilizadoresEmpresaAsync(int EmpresaID, List<EmpresaUtilizadoresViewModel> users)
        {
            try
            {
                using (context)
                {
                    users[0].EmpresaId = EmpresaID;   //nao esquecer resolver esta situacao
                    context.EmpresaUtilizadores.AddRange((IEnumerable<EmpresaUtilizadoresViewModel>)users.Where(x => x.IsSelected == true));
                    int iCount = await context.SaveChangesAsync();
                    return (iCount > 0) ? true : false;
                }
            }
            catch (DbUpdateException ex)
            {
                logger.LogCritical(ex.Message);
                return false;
            }
        }

        public async Task<bool> IsCompanyUserAsync(int EmpresaID, string userName)
        {
            try
            {
                using (context)
                {
                    int iCount = await context.EmpresaUtilizadores.Where(x => x.EmpresaId == EmpresaID && x.UserName == userName).CountAsync();
                    return (iCount > 0) ? true : false;
                }
            }
            catch (DbUpdateException ex)
            {
                logger.LogCritical(ex.Message);
                return false;
            }

        }

        public IEnumerable<EmpresasViewModel> GetActiveCabContabilidade()
        {
            return context.EmpresasViewModel.Where(x => x.isCabContabilidade == true).ToList();
        }

        public IEnumerable<EmpresasViewModel> GetEmpresasDaGabinete(int idEmprGabContab)
        {
            return context.EmpresasViewModel.Where(x => x.isCabContabilidade == false && x.IdCabContabilidade == idEmprGabContab).ToList();
        }

        public bool DeleteEmpresa(EmpresasViewModel _model)
        {
            var bResult = false;
            EmpresasViewModel _tmpModel = context.EmpresasViewModel.Where(x => x.EmpresaID == _model.EmpresaID).ToList()[0];
            if (_tmpModel != null)
            {
                context.EmpresasViewModel.Remove(_tmpModel);
                context.SaveChanges();
            }
            bResult = true;
            return bResult;
        }

        public IEnumerable<EmpresasViewModel> GetExistingRegistries()
        {
            return context.EmpresasViewModel.ToList();
        }

        public IEnumerable<EmpresasViewModel> GetModelByID(int id)
        {
            return context.EmpresasViewModel.Where(x => x.EmpresaID == id);
        }

        public IEnumerable<EmpresaUtilizadoresViewModel> GetUtilizadorEmpresa(string UserName, int EmpresaId)
        {
            return context.EmpresaUtilizadores.Where(x => x.UserName == UserName && x.EmpresaId == EmpresaId);
        }

        public List<ApplicationUser> GetUtilizadoresEmpresa(int EmpresaID)
        {

            List<string> tmp1 = context.EmpresaUtilizadores.Where(x => x.EmpresaId == EmpresaID).ToList().Select(x => x.UserID).ToList<string>();

            List<ApplicationUser> tmp = new List<ApplicationUser>();

            foreach (string v in tmp1)
            {
                ApplicationUser user = context.Users.FirstOrDefault(x => x.Id == v);
                tmp.Add(user);
            }
 
            return tmp;
 
        }

        public string InsertEmpresaJson(EmpresasViewModel _model)
        {
            if (_model.Nome == null) { return "success:false|'Nome da empresa é um campo obrigatório, não pode ser nulo'|NomeEmpresa"; };
            if (_model.NIF == null) { return "success:false|'NIF da empresa é um campo obrigatório, não pode ser nulo'|NifEmpresa"; };
            if (_model.Licenca == null) { return "success:false|'Licença software é um campo obrigatório, não pode ser nulo'|Licenca"; };
            if (_model.NrPostos == 0) { return "success:false|'Insira numero de licenças para empresa é um campo obrigatório, não pode ser nulo'|NrPostos"; };
            if (_model.NrEmpresas == 0) { return "success:false|'Numero de empresas é um campo obrigatório, não pode ser nulo'|NrEmpresas"; };
            if (_model.DataExpiracao == DateTime.Parse("01/01/0001 00:00:00")) { return "success:false|'A data da expiracao da licença é um campo obrigatório, não pode ser nulo'|DataExpiracao"; };
            try
            {
                context.EmpresasViewModel.Add(_model);
                int i = context.SaveChanges();
                return "success:true|error:'Registro inserido com sucesso'|"+ _model.EmpresaID + " ";
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Warning, "Method: Insert Impresa| Class: SQL_IEmpresa | Erro: " + ex.Message);
            }
            return "success:false|error:'O registro nao foi inserido'|0";
        }
        public string UpdateEmpresaJson(EmpresasViewModel _model)
        {
            if (_model.Nome == null) { return "success:false|'Nome da empresa é um campo obrigatório, não pode ser nulo'|NomeEmpresa"; };
            if (_model.NIF == null) { return "success:false|'NIF da empresa é um campo obrigatório, não pode ser nulo'|NifEmpresa"; };
            if (_model.Licenca == null) { return "success:false|'Licença software é um campo obrigatório, não pode ser nulo'|Licenca"; };
            if (_model.NrPostos == 0) { return "success:false|'Insira numero de licenças para empresa é um campo obrigatório, não pode ser nulo'|NrPostos"; };
            if (_model.NrEmpresas == 0) { return "success:false|'Numero de empresas é um campo obrigatório, não pode ser nulo'|NrEmpresas"; };
            if (_model.DataExpiracao == DateTime.Parse("01/01/0001 00:00:00")) { return "success:false|'A data da expiracao da licença é um campo obrigatório, não pode ser nulo'|DataExpiracao"; };
            try
            {
                var _tmp = context.EmpresasViewModel.Attach(_model);
                _tmp.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                int i = context.SaveChanges();
                return "success:true|error:'Registro actualizado com sucesso'|" + _model.EmpresaID + " ";
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Warning, "Method: Insert Impresa| Class: SQL_IEmpresa | Erro: " + ex.Message);
            }
            return "success:false|error:'O registro nao foi actualizado!'|" + _model.EmpresaID + " ";
        }
        public async Task<bool> InsertEmpresa(EmpresasViewModel _model)
        {
            var bResult = false;
            try
            {
                context.EmpresasViewModel.Add(_model);
                int i = context.SaveChanges();
                bResult = true;
                if (bResult)
                {//insert dados da empresa auxiliares.
                    IEnumerable<AGesEmpresasUtilizadores> tmpAGes = aGesContext.AGesEmpresasUtilizadores.Where(x => x.NIF == _model.NIF);

                    var tt = tmpAGes
                        .Select(x => new { x.AnoFi, x.AnoIn, x.CodeApplicacao, x.CodeEmpresa })
                        .Distinct().ToList();

                    foreach (var item in tt)
                    {
                        DadosEmpresaImportada tmp = new DadosEmpresaImportada()
                        {
                            EmpresaID = _model.EmpresaID,
                            AnoFi = item.AnoFi,
                            AnoIn = item.AnoIn,
                            CodeAplicacao = item.CodeApplicacao,
                            CodeEmpresa = item.CodeEmpresa
                        };
                        bool dadosExist = dadosEmpresaView.ModelExist(tmp);
                        if (dadosExist != true)
                        {
                            var result = await dadosEmpresaView.Insert(tmp);// .DadosEmpresaImportada.Where(x => x.== _model.NIF); 
                            if (result)
                            {
                                logger.Log(LogLevel.Information, "Dados Empresa inseridos com successo " + tmp.CodeEmpresa);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Warning, "Method: Insert Impresa| Class: SQL_IEmpresa | Erro: " + ex.Message);
            }
            return bResult;
        }

        public bool UpdateEmpresa(EmpresasViewModel _model)
        {
            var _tmp = context.EmpresasViewModel.Attach(_model);
            _tmp.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            bool bResult = true;
            return bResult;
        }


    }
}
//https://www.entityframeworktutorial.net/efcore/working-with-stored-procedure-in-ef-core.aspx

//list /combo
//https://www.pluralsight.com/guides/asp-net-mvc-populating-dropdown-lists-in-razor-views-using-the-mvvm-design-pattern-entity-framework-and-ajax