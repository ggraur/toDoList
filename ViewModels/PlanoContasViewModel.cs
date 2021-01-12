using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.ViewModels
{
	[Keyless]
    public class PlanoContasViewModel
    {
		public string CConta { get; set; }
		public string Tipo { get; set; }
		public string Descr { get; set; }
		public string CCIva { get; set; }
  
	}
}
