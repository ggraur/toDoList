using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.Models
{   
    [Keyless]
    public class AGesEmpresasUtilizadores
    {
        public string CodeEmpresa { get; set; }
        public string NomeEmpresa { get; set; }
        public string NIF { get; set; }
        public string CodeApplicacao { get; set; }
        public Int16 AnoIn { get; set; }
        public Int16 AnoFi { get; set; }
        public string Utilizador { get; set; }
    }
}
