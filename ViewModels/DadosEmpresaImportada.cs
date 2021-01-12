using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.ViewModels
{
    public class DadosEmpresaImportada
    {
        [Key]
        public int DEmpresaID { get; set; }
        public int EmpresaID { get; set; }
        public string CodeEmpresa { get; set; }
        public string CodeAplicacao { get; set; }
        public Int16 AnoIn { get; set; }
        public Int16 AnoFi { get; set; }
    }
}
