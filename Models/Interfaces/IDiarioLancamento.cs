using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.Models.Interfaces
{
    public interface IDiarioLancamento
    {
        public List<SelectListItem> GetDiarioLancemento();
        public List<SelectListItem> GetTipoLancamento();
        public List<SelectListItem> GetTipoDocumento();
    }
}
