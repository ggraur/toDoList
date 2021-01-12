using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoList.Models.SQL;
using toDoList.ViewModels;

namespace toDoList.Models
{
    public class GabContabilidadeRepository
    {
        private readonly AppDbContext context;

        public GabContabilidadeRepository(AppDbContext _context)
        {
            context = _context;
        }
        public IEnumerable<SelectListItem> GetGabContabilidade()
        {
            List<SelectListItem> gabContab = new List<SelectListItem>();
            SelectListItem newItem = new SelectListItem()
            {
                Value = "0",
                Text = "-- Selectione Gabinete --"
            };
            gabContab.Insert(0, newItem);
            var repo = new Sql_IGabineteContabilidade(context);
            if (repo!=null)
            {
                gabContab.RemoveAt(0);
                gabContab = repo.GetGabContabilidade().ToList();
                List<SelectListItem> tmp = gabContab.ToList<SelectListItem>();
                tmp.Insert(0, newItem);
                gabContab = tmp;
            }
            return new SelectList(gabContab, "Value", "Text");
        }

        internal IEnumerable<SelectListItem> GetEmprGabContabilidade(int idEmpresaGabContab)
        {
            List<SelectListItem> empresasGabContab = new List<SelectListItem>();
            SelectListItem newItem = new SelectListItem()
            {
                Value = "0",
                Text = "-- Selectione Empresa --"
            };
            empresasGabContab.Insert(0, newItem);
            var repo = new Sql_IGabineteContabilidade(context);
            if (repo != null)
            {
                empresasGabContab.RemoveAt(0);
                empresasGabContab = repo.GetEmprGabContabilidade(idEmpresaGabContab).ToList();
                List<SelectListItem> tmp = empresasGabContab.ToList<SelectListItem>();
                tmp.Insert(0, newItem);
                empresasGabContab = tmp;
            }
            return new SelectList(empresasGabContab, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetEmprGabContabilidadeAno(int idEmpresa)
        {
            //https://www.pluralsight.com/guides/asp-net-mvc-populating-dropdown-lists-in-razor-views-using-the-mvvm-design-pattern-entity-framework-and-ajax
            //aditionar 
            List<SelectListItem> anoEmpresasGabContab = new List<SelectListItem>();
            SelectListItem newItem = new SelectListItem()
            {
                Value = "0",
                Text = "-- Selectione Ano Fiscal --"
            };
            anoEmpresasGabContab.Insert(0, newItem);
            var repo = new Sql_IGabineteContabilidade(context);
            if (repo != null)
            {
                anoEmpresasGabContab.RemoveAt(0);
                anoEmpresasGabContab = repo.GetEmprGabContabilidadeAno(idEmpresa).ToList();
                List<SelectListItem> tmp = anoEmpresasGabContab.ToList<SelectListItem>();
                tmp.Insert(0, newItem);
                anoEmpresasGabContab = tmp;
            }
            return new SelectList(anoEmpresasGabContab, "Value", "Text");
        }

        public DadosEmpresaImportada GetEmpresaModel(int EmpresaID, Int16 Ano)
        {
            return context.DadosEmpresaImportada.FirstOrDefault(x => x.EmpresaID == EmpresaID && x.AnoFi==Ano);
        }
    }
}
