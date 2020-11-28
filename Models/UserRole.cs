using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.Models
{
    public class UserRole
    {
        public int RoleId { get; set; }
        public string RoleDesc { get; set; } // Admin//PowerUser//User
        public short Active { get; set; }
    }
}
