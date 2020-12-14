using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.ViewModels
{
    public class ConConfigViewModel
    {                           
        [Key]
        public int ConexaoID { get; set; }
        public int EmpresaId { get; set; }

        [Required(ErrorMessage = "O nome do servidor é um campo obrigatório")]
        [Display(Name = "Nome Servidor")]
        public string NomeServidor { get; set; }
        [Display(Name = "Nome da Instancia SQL")]
        public string InstanciaSQL { get; set; }
        [Required(ErrorMessage = "O nome do utilizador é um campo obrigatório")]
        [Display(Name = "Nome do Utilizador")]
        public string Utilizador { get; set; }
        [Required(ErrorMessage = "O palavra passe é um campo obrigatório")]
        [DataType(DataType.Password)]
        [Display(Name = "Palavra Passe")]
        public string Password { get; set; }
        [Display(Name = "Conexao Ativa")]
        public bool ActiveConnection { get; set; } = true;

    }
}
