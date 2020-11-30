using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoClassLibrary;

namespace toDoList.Models
{
    public class SQLToDoListRepository    : IToDoListRepository
    {
        private readonly AppDbContext context;

        public SQLToDoListRepository(AppDbContext context)
        {
            this.context = context;
        }

        public ToDoList Add(ToDoList toDoList)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ToDoTask> AddTaskList(IEnumerable<ToDoTask> toDoTasksList)
        {
            throw new NotImplementedException();
        }

        public ToDoList Details(int ToDoListID)
        {
            throw new NotImplementedException();
        }

        public ToDoList Update(ToDoList toDoList)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ToDoTask> UpdateTaskList(IEnumerable<ToDoTask> toDoTasksList)
        {
            throw new NotImplementedException();
        }
    }
}
