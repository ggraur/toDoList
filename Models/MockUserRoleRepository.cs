using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.Models
{
    public class MockUserRoleRepository : IUserRoleRepository
    {
        private List<UserRole> _usersRoleList;
        public MockUserRoleRepository()
        {
           _usersRoleList = new List<UserRole>()
            {
            new UserRole(){RoleId = 1, RoleDesc = "Admin",Active =1  },
            new UserRole(){RoleId = 2, RoleDesc = "PowerUser",Active =1  },
            new UserRole(){RoleId = 3, RoleDesc = "User",Active =1  }
            };

        }
  
        public UserRole GetUserRoleID(int RoleID)
        {
            return _usersRoleList.FirstOrDefault(e=>e.RoleId == RoleID);
        }
    }
}
