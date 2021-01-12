using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.ViewModels
{
    [Keyless]
    public class GeneralErrorViewModel
    {
        public string Signal { get; set; }
        public string ErrorTitle { get; set; }
        public string ErrorMessage { get; set; }
        public string StringButton { get; set; }
        public string UrlToRedirect { get; set; }
        public string optionalData { get; set; }
    }
}
