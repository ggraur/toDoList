using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.ViewModels
{
    [Keyless]
    public class CLabViewModel
    {

        [Display(Name = "ID Empresa Sage")]
        public int EmpresaSageId { get; set; }
        [Display(Name = "Ano Fiscal")]
        public Int16 Ano { get; set; }

        [Display(Name = "Data Lancamento")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DataLancamento { get; set; }

        [NotMapped]
        [Display(Name = "Caminho do ficheiro '.asc' ")]
        [Required, FileExtensions(Extensions = ".asc", ErrorMessage = "Formato de ficheiro incorrecto")]
        public IFormFile InputFilePath { get; set; }
        
        public string strInputFilePath { get; set; }
        public string InputFilePathStr { get; set; }

        [Display(Name = "Caminho do ficheiro 'Sage' ")]
        public string OutputFilePAth { get; set; }

        public string UserID { get; set; }

    }
}
