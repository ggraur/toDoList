using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.Models
{
    public class Task
    {
        public int TaskID { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription{ get; set; }
        public int WhoUserID { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime CompletitionDate { get; set; }
        public int AssignedToUserID { get; set; }
        public short TaskActive { get; set; }

    }
}
