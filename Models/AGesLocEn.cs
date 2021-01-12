using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.Models
{
    [Keyless]
    public class AGesLocEn
    {
        public string CLocE { get; set; }
        public string GesSharedDir { get; set; }
        public string SageSearchMachine { get; set; }
    }
}
