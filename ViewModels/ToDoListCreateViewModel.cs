using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoClassLibrary;

namespace toDoList.ViewModels
{
    public class ToDoListCreateViewModel
    {
        public int ToDoListID { get; set; }
        public string ToDoListName { get; set; }
        public IEnumerable<ToDoTask> ToDoTasks { get; set; }
        public DateTime CreatedToDoListDatetime { get; set; }
        public DateTime FinalizationDatetime { get; set; }
        public string UserIDCreator { get; set; }
        public string UserIDExecutor { get; set; }
    }
}
