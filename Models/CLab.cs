using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.Models
{
    [Keyless]
    public class CLab
    {
        [Display(Name = "ID Empresa Sage")]
        public int EmpresaSageId { get; set; }
        [Display(Name = "Ano Fiscal")]
        public Int16 Ano { get; set; }

        [Display(Name = "Data Lancamento")]
        public DateTime DataLancamento { get; set; }
        
        [Display(Name = "Caminho do ficheiro '.asc' ")]
        [Required, FileExtensions(Extensions = ".asc", ErrorMessage = "Formato de ficheiro incorrecto")]
        public string InputFilePath { get; set; }
        
        [Display(Name = "Caminho do ficheiro 'Sage' ")]
        public string OutputFilePAth { get; set; }

        public string UserID { get; set; }
    }
}
