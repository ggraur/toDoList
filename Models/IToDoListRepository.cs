using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoClassLibrary;
using toDoList.ViewModels;

namespace toDoList.Models
{
    public interface IToDoListRepository
    {
        public ToDoList Add(ToDoList toDoList);

        public List<AddTask_To_ToDoList> AddTasksToList(List<ToDoTask> tasks, ToDoList toDoList);

        public ToDoList Update(ToDoList toDoList);
        public ToDoList Delete(ToDoList toDoList);

        public IEnumerable<ToDoList> GetList();
        public IEnumerable<ToDoList> GetListById(int ListID);
        public IEnumerable<AddTask_To_ToDoList> GetToDoListById(int ListID);
        public IEnumerable<ToDoList> GetListByCreatorUser(int UserID);
        public IEnumerable<AddTask_To_ToDoList> GetListByAssignedUserId(int UserID);
        public IEnumerable<ToDoList> GetListByCreatorByAssignedToUser(int CreatorID, int UserAssignedID);
    }
}
