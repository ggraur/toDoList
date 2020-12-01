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
        public MyUser Add(MyUser user)
        {
            context.MyUsers.Add(user);
            context.SaveChanges();
            return user;
        }

        public MyUser Delete(int id)
        {
            MyUser _user = context.MyUsers.Find(id);
            if (_user != null)
            {
                context.MyUsers.Remove(_user);
                context.SaveChanges();
            }
            return _user;
        }

        public MyUser GetUserDetails(int UserID)
        {
            return context.MyUsers.Find(UserID);
        }

        public IEnumerable<MyUser> GetUsers()
        {
            return context.MyUsers;
        }

        public MyUser Update(MyUser userChanges)
        {
           var _user = context.MyUsers.Attach(userChanges);
            _user.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return userChanges;
        }
    }
}
