using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.Models
{
    public interface IDiarLancRepository
    {
        public List<SelectListItem> Diarios();
    }
}
