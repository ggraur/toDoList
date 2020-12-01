using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoClassLibrary;
using toDoList.ViewModels;

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
            context.ToDoLists.Add(toDoList);
            context.SaveChanges();
            return toDoList;
        }

        public ToDoList Delete(ToDoList toDoList)
        {
            context.ToDoLists.Attach(toDoList);
            context.ToDoLists.Remove(toDoList);
            context.SaveChanges();
            return toDoList;
        }

        public ToDoList Update(ToDoList toDoList)
        {
            var _todoList=context.ToDoLists.Attach(toDoList);
            _todoList.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return toDoList;
        }
        public IEnumerable<ToDoList> GetList()
        {
            return context.ToDoLists;
        }

        public IEnumerable<ToDoList> GetListById(int ListID)
        {
            return context.ToDoLists.Where(x=>x.ToDoListID == ListID);
        }

        public IEnumerable<AddTask_To_ToDoList> GetToDoListById(int ListID)
        {
            return context.AddTask_To_ToDoList.Where(x => x.ToDoListID == ListID);
        }

        public IEnumerable<ToDoList> GetListByCreatorUser(int UserID)
        {
            return context.ToDoLists.Where(x => x.IDCreator == UserID);
        }
        public IEnumerable<AddTask_To_ToDoList> GetListByAssignedUserId(int UserID)
        {
            return context.AddTask_To_ToDoList.Where(x => x.IDExecutor == UserID);
        }
        public IEnumerable<ToDoList>  GetListByCreatorByAssignedToUser(int CreatorID, int UserAssignedID)
        {
            return context.ToDoLists.Where(x => x.IDCreator == CreatorID && x.IDExecutor == UserAssignedID);
        }

        public List<AddTask_To_ToDoList> AddTasksToList(List<ToDoTask> tasks, ToDoList toDoList) 
        {
            List<AddTask_To_ToDoList> tmpAdd = new List<AddTask_To_ToDoList>();

            foreach (ToDoTask tsk in tasks )
            {
                AddTask_To_ToDoList tmp = new AddTask_To_ToDoList();
                tmp.TaskID = tsk.TaskID;
                tmp.ToDoListID = toDoList.ToDoListID;
                tmp.IDExecutor = toDoList.IDExecutor;
                tmp.IsChecked = true;
                tmp.TaskName = tsk.TaskName;
                tmp.TaskDescription = tsk.TaskDescription;
                tmp.TaskStatus = StatusTask.NotYet;
                context.AddTask_To_ToDoList.Add(tmp);
                context.SaveChanges();
                tmpAdd.Add(tmp);
            }
          
            return tmpAdd;
        }

       
    }
}
