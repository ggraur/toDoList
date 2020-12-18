using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace toDoList.Models
{
    public partial class EmprViewModel  
    {
        [Key]
        public string Cemp { get; set; }
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
        public string Ncontrib { get; set; }
        public decimal CapSocial { get; set; }
        public string ConsRegC { get; set; }
        public string Matriculado { get; set; }
        public string Cae { get; set; }
        public string Actividade { get; set; }
        public short RfinancasC { get; set; }
        public string RfinancasD { get; set; }
        public bool IsSelected { get; set; }

    }
}
