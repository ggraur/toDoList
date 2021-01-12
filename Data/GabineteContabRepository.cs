using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoList.Models;

namespace toDoList.Data
{
    public class GabineteContabRepository
    {
        private readonly IEmpresa dbContext;

        public GabineteContabRepository(IEmpresa _dbContext)
        {
            this.dbContext = _dbContext;
        }
        public IEnumerable<SelectListItem> GetGabContabilidade()
        {
            List<SelectListItem> gabinetesContab = dbContext.GetExistingRegistries().Where(x => x.isCabContabilidade == true)
                        .Select(x => new SelectListItem { Text = x.Nome, Value = x.EmpresaID.ToString() })
               .Select(n =>
                    new SelectListItem
                    {
                        Value = n.Value,
                        Text = n.Text
                    }).ToList();
            var tmp = new SelectListItem()
            {
                Value = null,
                Text = "--- Seleccione Gab. Contabilidade ---"
            };
            gabinetesContab.Insert(0, tmp);
            return new SelectList(gabinetesContab, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetEmpresasGabContabilidade(int idGabContabilidade)
        {
            List<SelectListItem> gabinetesContab = dbContext.GetExistingRegistries().Where(x => x.isCabContabilidade == false && x.IdCabContabilidade == idGabContabilidade)
                        .Select(x => new SelectListItem { Text = x.Nome, Value = x.EmpresaID.ToString() })
               .Select(n =>
                    new SelectListItem
                    {
                        Value = n.Value,
                        Text = n.Text
                    }).ToList();
            var tmp = new SelectListItem()
            {
                Value = null,
                Text = "--- Seleccione Empresa ---"
            };
            gabinetesContab.Insert(0, tmp);
            return new SelectList(gabinetesContab, "Value", "Text");
        }
    }
}

//https://www.pluralsight.com/guides/asp-net-mvc-populating-dropdown-lists-in-razor-views-using-the-mvvm-design-pattern-entity-framework-and-ajax