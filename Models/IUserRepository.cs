using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.Models
{
    public interface IUserRepository
    {
        public User GetUserDetails(int UserID);
        public IEnumerable<User> GetUsers();
        public User Add(User user);
    }
}
