using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using toDoClassLibrary;

namespace toDoList.ViewModels
{
    public class AddTask_To_ToDoList
    {                                        
        [Key]
        public int ListTaskID { get; set; }
        public int ToDoListID { get; set; }
        [Display(Name = "Checked")]
        public bool IsChecked { get; set; }
        public int TaskID { get; set; }
        [Display(Name = "Task Name")]
        public string TaskName { get; set; }
        [Display(Name = "Task Description")]
        public string TaskDescription { get; set; }
        [Display(Name = "Status")]
        public StatusTask TaskStatus { get; set; }

        public int IDExecutor { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}
