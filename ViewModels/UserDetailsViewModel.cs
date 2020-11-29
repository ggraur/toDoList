using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoList.Models;
using toDoClassLibrary; 

namespace toDoList.ViewModels
{
    public class UserDetailsViewModel
    {
        public User User { get; set; }
        //public IEnumerable<User> GetUsers { get;set;}
        public string PageTitle { get; set; }
    }
}
