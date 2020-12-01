﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoClassLibrary;

namespace toDoList.Models
{
    public interface IToDoListRepository
    {
        public ToDoList Add(ToDoList toDoList);
        public ToDoList Update(ToDoList toDoList);
        public ToDoList Delete(ToDoList toDoList);
    }
}
