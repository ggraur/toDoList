using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.Models
{
    public class User
    {
        public int UserID { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string UserPass { get; set; }
        [Required]
        public UserRoleEnum UserRole { get; set; }
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }

    }
}
