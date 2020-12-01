using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoClassLibrary;

namespace toDoList.Models
{
    public class SQLTaskRepository : ITaskRepository
    {
        private readonly AppDbContext context;
        public SQLTaskRepository(AppDbContext _context)
        {
            this.context = _context;
        }
        public ToDoTask Add(ToDoTask task)
        {
            context.Add(task);
            context.SaveChanges();
            return task;
        }

        public ToDoTask Details(int TaskId)
        {
            return context.Tasks.Find(TaskId);
        }

        public IEnumerable<ToDoTask> Tasks()
        {
            return context.Tasks;
        }

        public IEnumerable<ToDoTask> ActiveTasks()
        {
            return context.Tasks.Where(x=>x.TaskActive==0); //0- active, 1- inactive
        }

        public ToDoTask Update(ToDoTask taskChanges)
        {
            var _task = context.Tasks.Attach(taskChanges);
            _task.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return taskChanges;
        }
        public ToDoTask Delete(ToDoTask task)
        {
            context.Attach(task);
            context.Remove(task);
            context.SaveChanges();
            return task;
        }
    }
}
