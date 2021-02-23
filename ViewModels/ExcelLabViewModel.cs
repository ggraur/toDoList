using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using toDoClassLibrary;

namespace toDoList.ViewModels
{
[Keyless]
    public class ExcelLabViewModel
    {
        [Display(Name = "Código da Empresa")]
        public string CodEmpresa { get; set; }
        [Display(Name = "Ano de Lançamento")]
        public Int16 AnoLancamento { get; set; }
        [Display(Name = "Mês de Lançamento")]
        public MonthEnum MesLancamento { get; set; }
        [NotMapped]
        public string MesLancamentoStr { get; set; }

        [Display(Name = "Diário de Lançamento")]
        public IEnumerable<SelectListItem> DiarioLancamento { get; set; }

        [NotMapped]
        public string DiarioLancamentoInt { get; set; }

        [NotMapped]
        public string  DiarioLancamentoStr { get; set; }

        [Display(Name = "Tipo de Lançamento")] 
        public IEnumerable<SelectListItem> TipoLancamento { get; set; }

        [NotMapped]
        public string TipoLancamentoInt { get; set; }

        [NotMapped]
        public string TipoLancamentoStr { get; set; }
        [Display(Name = "Tipo de Documento")]
        public IEnumerable<SelectListItem> TipoDocumento { get; set; }
        
        [NotMapped]
        public string TipoDocumentoInt { get; set; }
        
        [NotMapped]
        public string TipoDocumentoStr { get; set; }

        [Display(Name = "Data Lançamento")]
        public DateTime DataLancamento { get; set; }

        [Display(Name = "Lançamento Único")]
        public bool LancamentoUnico { get; set; }
        [Display(Name = "Ficheiro de Input")]
        public string InputFilePath { get; set; }
        [Display(Name = "Diretoria de Output")]
        public string OutputFilePath { get; set; }
    }
}
