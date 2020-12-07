using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual AppUserAddress AppUserAddress { get; set; }
    }

    public class AppUserAddress
    {
        [Key]
        public int UserAddressId { get; set; }
        [ForeignKey("AspNetUsers")]
        public string IdUser { get; set; }
        public string Country { get; set; }
        public string District { get; set; }
        public string Municipality { get; set; }
        public string City { get; set; }
        public string Gender { get; set; }
    }
}
