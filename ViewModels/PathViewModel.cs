using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.ViewModels
{
    [Keyless]
    public class PathViewModel
    {
        public int EmpresaID { get; set; }
        public Int16 Ano { get; set; }

        [Display(Name = "Diretoria de exportação:")] 
        public string Path { get; set; }
    }
}
