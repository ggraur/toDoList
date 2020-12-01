using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoClassLibrary;

namespace toDoList.Models
{
    public interface IUserRepository
    {
        public MyUser GetUserDetails(int UserID);
        public IEnumerable<MyUser> GetUsers();
        public MyUser Add(MyUser user);
        public MyUser Update(MyUser user);
    }
}
