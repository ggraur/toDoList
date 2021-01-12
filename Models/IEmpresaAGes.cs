using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoList.ViewModels;

namespace toDoList.Models
{
    public interface IEmpresaAGes
    {
        public IEnumerable<AGesEmpresasUtilizadores> GetUtilizadores(string NIF, string NomeEmpresa);
        public AGesLocEn  GetAGesLocEn();
    }
}
