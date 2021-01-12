using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.ViewModels
{
    [Keyless]
    public class AGesEmprsViewModel
    {
        public string CEmp { get; set; }
        public string Nome { get; set; }
        public string Morada { get; set; }
        public string Localidade { get; set; }
        public string CodPostal { get; set; }
        public string Distrito { get; set; }
        public string Concelho { get; set; }
        public string Freguesia { get; set; }
        public string Sede { get; set; }
        public string Telefone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string NContrib { get; set; }
        public double CapSocial { get; set; }
        public string ConsRegC { get; set; }
        public string Matriculado { get; set; }
        public string CAE { get; set; }
        public string Actividade { get; set; }
        public Int16 RFinancasC { get; set; }
        public string RFinancasD { get; set; }
    }
}
