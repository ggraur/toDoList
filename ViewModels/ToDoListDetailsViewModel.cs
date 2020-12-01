using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoClassLibrary;

namespace toDoList.ViewModels
{
    public class ToDoListDetailsViewModel
    {
        public ToDoList ToDoList { get; set; }

        public IEnumerable<ToDoTask> Tasks {get;set;}
        public string PageTitle { get; set; }
    }
}
