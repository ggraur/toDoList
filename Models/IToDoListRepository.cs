using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoClassLibrary;

namespace toDoList.Models
{
    public interface IToDoListRepository
    {
        public ToDoList Details(int ToDoListID);
        public ToDoList Add(ToDoList toDoList);
        public ToDoList Update(ToDoList toDoList);
        public IEnumerable<ToDoTask> UpdateTaskList(IEnumerable<ToDoTask> toDoTasksList);
        public IEnumerable<ToDoTask> AddTaskList(IEnumerable<ToDoTask> toDoTasksList);

    }
}
