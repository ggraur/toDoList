using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using toDoClassLibrary;

namespace toDoList.ViewModels
{
    public class TaskCreateViewModel
    {           
        [Key]
        public int TaskID { get; set; }
        [Required]
        public string TaskName { get; set; }
        [Required]
        public string TaskDescription { get; set; }
        public StatusEnum TaskActive { get; set; }

        public string pp { get; set; }
    }
}
