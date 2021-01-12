using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.Models
{
    [Keyless]
    public class mapView_GabContabilidade
    {
        public int EmpresaID { get; set; }
        public string Nome { get; set; }
        public string NIF { get; set; }
        public string Licenca { get; set; }
        public int NrPostos { get; set; }
        public int NrEmpresas { get; set; }
        public DateTime DataExpiracao { get; set; }
        public DateTime DataCriacao { get; set; }
        public bool Ativo { get; set; }

    }
}
