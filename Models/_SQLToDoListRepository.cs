using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoClassLibrary;
using toDoList.ViewModels;

namespace toDoList.Models
{
    public class SQLToDoListRepository    : IToDoListRepository
    {
        private readonly AppDbContext context;

        public SQLToDoListRepository(AppDbContext context)
        {
            this.context = context;
        }

        public ToDoListCreateViewModel Add(ToDoListCreateViewModel toDoList)
        {
            context.Add(toDoList);
            context.SaveChanges();
            return toDoList;
        }

        public IEnumerable<ToDoTask> AddTaskList(IEnumerable<ToDoTask> toDoTasksList)
        {
            context.Add(toDoTasksList);
            context.SaveChanges();
            return toDoTasksList;
        }

        public IEnumerable<ToDoList> Details()
        {
            return context.ToDoLists;
        }
        public ToDoList Details(int ToDoListID)
        {
            return context.ToDoLists.Find(ToDoListID);
        }

        public ToDoList Update(ToDoList toDoList)
        {
            var _todoList = context.ToDoLists.Attach(toDoList);
            _todoList.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return toDoList;
        }

        public ToDoList Delete(ToDoList toDoList)
        {
            context.Attach(toDoList);
            context.Remove(toDoList);
            context.SaveChanges();
            return toDoList;
        }

        
    }
}
