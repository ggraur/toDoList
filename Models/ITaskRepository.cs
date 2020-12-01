using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoClassLibrary;

namespace toDoList.Models
{
    public interface ITaskRepository
    {
        public ToDoTask Details(int TaskId);
        public IEnumerable<ToDoTask> Tasks();
        public IEnumerable<ToDoTask> ActiveTasks();
        public ToDoTask Add(ToDoTask task);
        
        public ToDoTask Update(ToDoTask task);
        public ToDoTask Delete(ToDoTask task);
    }
}
