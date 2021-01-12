using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoList.ViewModels;

namespace toDoList.Models.Interfaces
{
   public interface IGabineteContabilidade
    {
        public List<SelectListItem> GetGabContabilidade();
        public List<SelectListItem> GetEmprGabContabilidade(int EmpresaID);
        public List<SelectListItem> GetEmprGabContabilidadeAno(int EmpresaID);
        public EmpresasViewModel GetEmpresaModel(int EmpresaID);
    }
}
