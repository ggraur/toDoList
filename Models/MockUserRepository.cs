using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.Models
{
    public class MockUserRepository : IUserRepository
    {
        private List<User> _usersList;
         public MockUserRepository()
        {
            _usersList = new List<User>()
            {
            new User(){UserID = 1, UserName = "Mary" , UserPass ="maryPass" , RoleId = 1, UserEmail ="mary@gmail.com" },
            new User(){UserID = 2, UserName = "Karl" , UserPass ="karlPass" , RoleId = 2, UserEmail ="Karl@gmail.com" },
            new User(){UserID = 3, UserName = "Eric" , UserPass ="ericPass" , RoleId = 3, UserEmail ="Eric@gmail.com" },
            new User(){UserID = 4, UserName = "Jorge", UserPass ="jorgePass", RoleId = 2, UserEmail ="Jorge@gmail.com" },
            new User(){UserID = 5, UserName = "Ann"  , UserPass ="annPass"  , RoleId = 2, UserEmail ="Ann@gmail.com" }
            };
        }

      

        public IEnumerable<User> GetUsers()
        {
            return _usersList;
        }

        public User GetUserDetails(int Id)
        {
            var b = _usersList.FirstOrDefault(e => e.UserID == Id);
            return b;
        }
    }
}
