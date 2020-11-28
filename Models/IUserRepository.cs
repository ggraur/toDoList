using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.Models
{
    public interface IUserRepository
    {
       public User GetUser(int UserID);
       public IEnumerable<User> GetUsers();
    }
}
