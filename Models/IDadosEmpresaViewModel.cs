using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoList.ViewModels;

namespace toDoList.Models
{
    public interface IDadosEmpresaViewModel 
    {
        public Task<bool> Insert(DadosEmpresaImportada model);
        public Task<bool> Update(DadosEmpresaImportada model);
        public bool ModelExist(DadosEmpresaImportada model);

        public DadosEmpresaImportada ReturnModelByEmpresaAno(int EmpresaID, Int16 Ano);
    }
}
