using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoClassLibrary;

namespace toDoList.Models
{
    public class MockUserRepository : IUserRepository
    {
        private List<User> _usersList;
        public MockUserRepository()
        {
            _usersList = new List<User>()
            {
            new User(){UserID = 1, UserName = "Mary" , UserPass ="maryPass" , UserRole = UserRoleEnum.Administrator, UserEmail ="mary@gmail.com" },
            new User(){UserID = 2, UserName = "Karl" , UserPass ="karlPass" , UserRole = UserRoleEnum.PowerUser, UserEmail ="Karl@gmail.com" },
            new User(){UserID = 3, UserName = "Eric" , UserPass ="ericPass" , UserRole = UserRoleEnum.PowerUser, UserEmail ="Eric@gmail.com" },
            new User(){UserID = 4, UserName = "Jorge", UserPass ="jorgePass", UserRole = UserRoleEnum.Guest, UserEmail ="Jorge@gmail.com" },
            new User(){UserID = 5, UserName = "Ann"  , UserPass ="annPass"  , UserRole = UserRoleEnum.User, UserEmail ="Ann@gmail.com" }
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

        public User Add(User user)
        {
            user.UserID = _usersList.Max(e => e.UserID + 1);
            _usersList.Add(user);
            return user;
        }

        public User Update(User userChanges)
        {
            User _user = _usersList.FirstOrDefault(x => x.UserID == userChanges.UserID);
            if (_user != null)
            {
                _user.UserName = userChanges.UserName;
                _user.UserPass = userChanges.UserPass;
                _user.UserRole = userChanges.UserRole;
                _user.UserEmail = userChanges.UserEmail;
            }
            return _user;
        }

        public User Delete(int id)
        {
            User user = _usersList.FirstOrDefault(x => x.UserID == id);
            if (user != null)
            {
                _usersList.Remove(user);
            }
            return user;
        }
    }
}
