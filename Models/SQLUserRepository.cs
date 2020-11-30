using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoClassLibrary;

using Microsoft.EntityFrameworkCore;

namespace toDoList.Models
{
    public class SQLUserRepository : IUserRepository
    {
        private readonly AppDbContext context;

        public SQLUserRepository(AppDbContext context)
        {
            this.context = context;
        }
        public User Add(User user)
        {
            context.Users.Add(user);
            context.SaveChanges();
            return user;
        }

        public User Delete(int id)
        {
            User _user = context.Users.Find(id);
            if (_user != null)
            {
                context.Users.Remove(_user);
                context.SaveChanges();
            }
            return _user;
        }

        public User GetUserDetails(int UserID)
        {
            return context.Users.Find(UserID);
        }

        public IEnumerable<User> GetUsers()
        {
            return context.Users;
        }

        public User Update(User userChanges)
        {
           var _user = context.Users.Attach(userChanges);
            _user.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return userChanges;
        }
    }
}
