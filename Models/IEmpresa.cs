using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoList.ViewModels;

namespace toDoList.Models
{
    public interface IEmpresa
    {
        public bool InsertEmpresa(EmpresasViewModel _model);
        public bool UpdateEmpresa(EmpresasViewModel _model);
        public bool DeleteEmpresa(EmpresasViewModel _model);

        public IEnumerable<EmpresasViewModel> GetModelByID(int id);
        public IEnumerable<EmpresasViewModel> GetExistingRegistries();
        public List<ApplicationUser> GetUtilizadoresEmpresa(int EmpresaID);
        public Task<bool> RemoveFromUtilizadoresEmpresaAsync(int EmpresaID);
        public Task<bool> AddUtilizadoresEmpresaAsync(int EmpresaID, List<EmpresaUtilizadoresViewModel> users);
        public Task<bool> IsCompanyUserAsync(int EmpresaID, string userName);

    }
}
