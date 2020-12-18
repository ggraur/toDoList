using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.Models
{
    public class CLab
    {
        [Key]
        public int IdCLab { get; set; }
        public int EmpresaSageId { get; set; }
        public Int16 Ano { get; set; }
        public DateTime DataLancamento { get; set; }
        public string InputFilePAth { get; set; }
        public string OutputFilePAth { get; set; }
        public string UserID { get; set; }
    }
}
