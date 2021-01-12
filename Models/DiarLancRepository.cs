using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.Models
{
    [Keyless]
    public class DiarLancRepository
    {
        public int Diario_ID { get; set; }
        public string Diario { get; set; }
    }
}
