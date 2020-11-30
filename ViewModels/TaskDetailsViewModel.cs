using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoClassLibrary;

namespace toDoList.ViewModels
{
    public class TaskDetailsViewModel
    {
        public ToDoTask ToDoTask { get; set; }

        public string PageTitle { get; set; }
    }
}
