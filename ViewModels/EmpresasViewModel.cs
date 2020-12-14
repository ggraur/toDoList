using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.ViewModels
{
    public class EmpresasViewModel
    {
        [Key]
        public int EmpresaID { get; set; }
        [Required(ErrorMessage = "O nome da empresa é um campo obrigatório")]
        [Display(Name = "Nome Empresa")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O NIF da empresa é um campo obrigatório")]
        [Display(Name = "NIF Empresa")]
        public string NIF { get; set; }

        [Display(Name = "Licença")]
        public string Licenca { get; set; }
        
        [Display(Name = "Número de Postos da licença")]
        public string NrPostos { get; set; }
        [Display(Name = "Número de Empresas")]
        public int NrEmpresas { get; set; }
        [Display(Name = "Data de expiração da licença")]
        public DateTime DataExpiracao { get; set; }
        [Display(Name = "Data de criação do registro")]

        public DateTime DataCriacao { get; set; } = DateTime.Now;
        [Display(Name = "Registro da empresa Activo/Inactivo")]
        public bool Ativo { get; set; } = true;
    }
}
