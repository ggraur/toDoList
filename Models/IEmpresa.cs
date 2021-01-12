using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoList.ViewModels;

namespace toDoList.Models
{
    public interface IEmpresa
    {
        public Task<bool> InsertEmpresa(EmpresasViewModel _model);
        public string InsertEmpresaJson(EmpresasViewModel _model);
        public string UpdateEmpresaJson(EmpresasViewModel _model);
        public bool UpdateEmpresa(EmpresasViewModel _model);
        public bool DeleteEmpresa(EmpresasViewModel _model);
        public IEnumerable<EmpresasViewModel> GetActiveCabContabilidade();
        public IEnumerable<EmpresasViewModel> GetModelByID(int id);
        public IEnumerable<EmpresasViewModel> GetExistingRegistries();
        public IEnumerable<EmpresasViewModel> GetEmpresasDaGabinete(int idEmprGabContab);
        public IEnumerable<EmpresaUtilizadoresViewModel> GetUtilizadorEmpresa(string UserName, int EmpresaId);
        public List<ApplicationUser> GetUtilizadoresEmpresa(int EmpresaID);
        public Task<bool> RemoveFromUtilizadoresEmpresaAsync(int EmpresaID);
        public Task<bool> AddUtilizadoresEmpresaAsync(int EmpresaID, List<EmpresaUtilizadoresViewModel> users);
        public Task<bool> AddUtilizadorEmpresaAsync(int EmpresaID, EmpresaUtilizadoresViewModel user);
        public Task<bool> IsCompanyUserAsync(int EmpresaID, string userName);
        public Task<bool> RemoveUtilizadorFromEmpresaAsync(EmpresaUtilizadoresViewModel tmpUser);

        public  int  ReturnCompanyID(string nomeEmpresa, string NIF);

    }
}
