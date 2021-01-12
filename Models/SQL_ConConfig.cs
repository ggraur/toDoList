using toDoClassLibrary47;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoList.ViewModels;

namespace toDoList.Models
{
    public class SQL_ConConfig : IConConfig
    {
        private readonly AppDbContext context;

        public SQL_ConConfig(AppDbContext context)
        {
            this.context = context;
        }

        //public async Task<ConConfigViewModel> GetModelByID(int id) 
        //{
        //    var taskResult = await Task.Run(() =>
        //    {
        //        return context.ConConfigViewModel.Where(x => x.EmpresaID == id);
        //    });
        //    return (ConConfigViewModel)taskResult;
        //}

        public bool DeleteConnection(ConConfigViewModel _model)
        {
            var bResult = false;
            ConConfigViewModel _tmpModel = context.ConConfigViewModel.FirstOrDefault(x => x.ConexaoID == _model.ConexaoID); 
            if (_tmpModel != null)
            {
                context.ConConfigViewModel.Remove(_tmpModel);
                context.SaveChanges();
                bResult = true;
            }
            
            return bResult;
        }

        public string InsertConnection(ConConfigViewModel _model)
        {
            if (_model.EmpresaId == 0) { return "success:false|'Id Empresa é um campo obrigatório, não pode ser nulo'|EmpresaId"; };
            if (_model.NomeServidor == null) { return "success:false|'NomeServidor é um campo obrigatório, não pode ser nulo'|NomeServidor"; };
            if (_model.Utilizador == null) { return "success:false|'Utilizador é um campo obrigatório, não pode ser nulo'|Utilizador"; };
            if (_model.Password == null) { return "success:false|'Palavra Passe é um campo obrigatório, não pode ser nulo'|Password"; };

            EncryptionHelper encryptionHelper = new EncryptionHelper();
            _model.Password = encryptionHelper.Encrypt(_model.Password);
            context.ConConfigViewModel.Add(_model);
            context.SaveChanges();
            return "success:true|error:'Registro inserido com sucesso'|0";
        }

        public string UpdateConnection(ConConfigViewModel _model)
        {
            if (_model.ConexaoID == 0) { return "success:false|'Id Conexao é um campo obrigatório, não pode ser nulo'|ConexaoID"; };
            if (_model.EmpresaId == 0) { return "success:false|'Id Empresa é um campo obrigatório, não pode ser nulo'|EmpresaId"; };
            if (_model.NomeServidor == null) { return "success:false|'NomeServidor é um campo obrigatório, não pode ser nulo'|NomeServidor"; };
            if (_model.Utilizador == null) { return "success:false|'Utilizador é um campo obrigatório, não pode ser nulo'|Utilizador"; };
            if (_model.Password ==null){return "success:false|'Palavra Passe é um campo obrigatório, não pode ser nulo'|Password"; }; 

            EncryptionHelper encryptionHelper = new EncryptionHelper();
            _model.Password = encryptionHelper.Encrypt(_model.Password);
            
            var _tmp = context.ConConfigViewModel.Attach(_model);
            _tmp.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return "success:true|error:'Registro atualizado com sucesso'|0"; 
        }

        public IEnumerable<ConConfigViewModel> GetExistingRegistries(int EmpresaId)
        {
            return context.ConConfigViewModel.Where(x=>x.EmpresaId==EmpresaId).ToList();
        }

        public IEnumerable<ConConfigViewModel> FindByEmpresaId(int EmpresaId)
        {
            return context.ConConfigViewModel.Where(x => x.EmpresaId == EmpresaId);
        }
    public IEnumerable<ConConfigViewModel> FindById(int id)
        {
            return context.ConConfigViewModel.Where(x => x.ConexaoID == id);
        }
        public async Task<ConConfigViewModel> FindByIdAsync(int id)
        {
            var taskResult = await Task.Run(() =>
            {
                return context.ConConfigViewModel.Where(x => x.ConexaoID == id);
            });
            return (ConConfigViewModel)taskResult;
        }

        public ConConfigViewModel GetModelByID(int id)
        {
           return context.ConConfigViewModel.Where(x => x.ConexaoID == id).ToList()[0];
        }

        public IEnumerable< ConConfigViewModel> IenumGetModelByID(int id)
        {
            return context.ConConfigViewModel.Where(x => x.ConexaoID == id);
        }
    }


}
