using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoList.Models.Interfaces;
using toDoList.ViewModels;

namespace toDoList.Models.SQL
{
    
    public class Sql_IGabineteContabilidade : IGabineteContabilidade
    {
        private readonly AppDbContext context;
        public Sql_IGabineteContabilidade(AppDbContext _context)
        {
            context = _context;
        }
        public List<SelectListItem> GetGabContabilidade()
        {
            List<SelectListItem> tmp = new List<SelectListItem>();
            using (context) 
            {
                var tt = context.View_GabContabilidade.ToList();
                foreach (var item in tt)
                {
                    SelectListItem listItem = new SelectListItem()
                    {
                       Value = item.EmpresaID.ToString(),
                       Text = item.Nome
                    };
                    tmp.Add(listItem);
                }

                return tmp;
            }
        }
        public List<SelectListItem> GetEmprGabContabilidade(int EmpresaID)
        {
            List<SelectListItem> tmp = new List<SelectListItem>();
            using (context)
            {
                var tt = context.View_EmpresasGabContabilidade.Where(x => x.IdCabContabilidade == EmpresaID).ToList();
                foreach (var item in tt)
                {
                    SelectListItem listItem = new SelectListItem()
                    {
                        Value = item.EmpresaID.ToString(),
                        Text = item.Nome
                    };
                    tmp.Add(listItem);
                }

                return tmp;
            }
        }
 
        public List<SelectListItem> GetEmprGabContabilidadeAno(int EmpresaID)
        {
            List<SelectListItem> tmp = new List<SelectListItem>();
            using (context)
            {
                var tt = context.View_EmpresasGabContabilidadeAno.Where(x => x.EmpresaId == EmpresaID).ToList();
                foreach (var item in tt)
                {
                    SelectListItem listItem = new SelectListItem()
                    {
                        Value = item.EmpresaId.ToString(),
                        Text = item.AnoFi.ToString()
                    };
                    tmp.Add(listItem);
                }
                return tmp;
            }
        }

        public EmpresasViewModel GetEmpresaModel(int EmpresaID) 
        {
            return context.EmpresasViewModel.FirstOrDefault(x => x.EmpresaID == EmpresaID);
        }
    }

  
}
