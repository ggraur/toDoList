using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoClassLibrary;

namespace toDoList.Models
{
    public class SQLToDoRepository : IToDoListRepository
    {

        private readonly AppDbContext context;

        public SQLToDoRepository(AppDbContext context)
        {
            this.context = context;
        }
        public ToDoList Add(ToDoList toDoList)
        {
            throw new NotImplementedException();
        }

        public ToDoList Delete(ToDoList toDoList)
        {
            throw new NotImplementedException();
        }

        public ToDoList Update(ToDoList toDoList)
        {
            throw new NotImplementedException();
        }
    }
}
