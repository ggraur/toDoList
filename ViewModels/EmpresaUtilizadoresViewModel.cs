using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.ViewModels
{
    public class EmpresaUtilizadoresViewModel
    {
        [Key]
        public int Id { get; set; }
        public int EmpresaId { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public bool IsSelected { get; set; }

    }
}
