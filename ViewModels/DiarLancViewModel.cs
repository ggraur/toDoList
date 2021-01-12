using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.ViewModels
{
    [Keyless]
    public class DiarLancViewModel
    {
        public int DR_ID { get; set; }
        public string DR_Descricao { get; set; }
    }
}
