using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Token { get; set; }

        public DateTime ResetLinkCreatedTime { get; set; }
        public DateTime ResetLinkValidity { get; set; }

        public DateTime ResetLinkConfirmationDate { get; set; }

    }
}
