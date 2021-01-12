using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
 

namespace toDoList.ViewModels
{
    [Keyless]
    public class GabineteEditViewModel
    {
        [Required]
        [Display(Name ="Gabinete Contabilidade")]
        public int EmpresaContabID { get; set; }
        public IEnumerable<SelectListItem> EmpresasContabilidade { get; set; }

        [Required]
        [Display(Name = "Empresa")]
        public int EmpresaID { get; set; }
        public IEnumerable<SelectListItem> Empresas { get; set; }

        [Required]
        [Display(Name = "Ano Fiscal")]
        public string CodeEmpresa { get; set; }
        public IEnumerable<SelectListItem> AnoFiscal { get; set; }
    }
}
