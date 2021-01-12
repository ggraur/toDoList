using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.Models
{
    [Keyless]
    public class mapView_EmpresasGabContabilidadeAno
    {
        public int EmpresaId { get; set; }
        public Int16 AnoFi { get; set; }
    }
}
