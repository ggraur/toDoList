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
        public User GetUserDetails(int UserID);
        public IEnumerable<User> GetUsers();
        public User Add(User user);
        public User Update(User user);
        public User Delete(int id);
    }
}
