﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoClassLibrary;
using toDoList.ViewModels;

namespace toDoList.Models
{
    public interface IToDoListRepository
    {
        public IEnumerable<ToDoList> Details();
        public ToDoList Details(int ToDoListID);
        public ToDoListCreateViewModel Add(ToDoListCreateViewModel toDoList);
        public ToDoList Update(ToDoList toDoList);
        public IEnumerable<ToDoTask> AddTaskList(IEnumerable<ToDoTask> toDoTasksList);
        public ToDoList Delete(ToDoList toDoList);
    }
}
