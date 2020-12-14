using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoList.ViewModels;

namespace toDoList.Models
{
    public interface IEmpresaUtilizadores
    {
        public bool InsertUserEmpresa(EmpresaUtilizadoresViewModel _model);
        public bool UpdateUserEmpresa(EmpresaUtilizadoresViewModel _model);
        public bool DeleteUserEmpresa(EmpresaUtilizadoresViewModel _model);
    }
}
