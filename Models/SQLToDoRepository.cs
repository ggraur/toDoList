﻿using System;
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

        public  AddTask_To_ToDoList  Update(AddTask_To_ToDoList addTask_To_ToDoList)
        {
            var _task = context.AddTask_To_ToDoList.Attach(addTask_To_ToDoList);
            _task.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return addTask_To_ToDoList;
        }
        public IEnumerable<ToDoList> GetList()
        {
            return context.ToDoLists;
        }

        public AddTask_To_ToDoList DeleteListItemByIdItem(int ItemID)
        {
            AddTask_To_ToDoList _tdItem = context.AddTask_To_ToDoList.Find(ItemID);
            if (_tdItem != null)
            {
                context.AddTask_To_ToDoList.RemoveRange(_tdItem);
                context.SaveChanges();
            }
            return _tdItem;
        }
        public IEnumerable<AddTask_To_ToDoList> GetListItemByIdItem(int ItemID)
        {
            return context.AddTask_To_ToDoList.Where(x => x.ListTaskID == ItemID);
        }
        public IEnumerable<ToDoList> GetListById(int ListID)
        {
            return context.ToDoLists.Where(x=>x.ToDoListID == ListID);
        }

        public IEnumerable<AddTask_To_ToDoList> GetToDoListById(int ListID)
        {
            return context.AddTask_To_ToDoList.Where(x => x.ListTaskID == ListID);
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
                tmp.TaskStatus = StatusTask.Created;
                tmp.CreatedDate = toDoList.CreatedToDoListDatetime;
                tmp.UpdatedDate = DateTime.Now;
                context.AddTask_To_ToDoList.Add(tmp);
                context.SaveChanges();
                tmpAdd.Add(tmp);
            }
          
            return tmpAdd;
        }

       
    }
}
