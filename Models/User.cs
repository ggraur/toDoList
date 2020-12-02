using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using toDoClassLibrary;
using toDoList.ViewModels;

namespace toDoList.Models
{
    public class MyUser: LoginViewModel
    {
        public int UserID { get; set; }

        [Required]
        [MaxLength(30,ErrorMessage ="Name cannot exceed 50 characters")]
        public string UserName { get; set; }
        
        [Required]
        public string UserPass { get; set; }
        
        [Required]
        public UserRoleEnum UserRole { get; set; }
        
        [Required]
        [EmailAddress]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid e-mail format, please correct it!")]
        public string UserEmail { get; set; }

    }
}
