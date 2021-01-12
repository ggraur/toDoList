using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoList.ViewModels;

namespace toDoList.Models
{
    public interface IConConfig
    {
        public string InsertConnection(ConConfigViewModel _model);
        public string UpdateConnection(ConConfigViewModel _model);
        public bool DeleteConnection(ConConfigViewModel _model);

        public IEnumerable<ConConfigViewModel> FindByEmpresaId(int EmpresaId);
        public IEnumerable<ConConfigViewModel> FindById(int id);
        public ConConfigViewModel GetModelByID(int id);
        public IEnumerable<ConConfigViewModel> IenumGetModelByID(int id);
        public IEnumerable<ConConfigViewModel> GetExistingRegistries(int EmpresaId);

    }
}
