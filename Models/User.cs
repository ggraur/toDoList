using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserPass { get; set; }
        public int RoleId { get; set; }
        public string UserEmail { get; set; }

    }
}
