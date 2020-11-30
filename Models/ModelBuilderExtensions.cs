using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoClassLibrary;

namespace toDoList.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                    new User() { UserID = 1, UserName = "Mary", UserPass = "maryPass", UserRole = UserRoleEnum.Administrator, UserEmail = "mary@gmail.com" },
                    new User() { UserID = 2, UserName = "Karl", UserPass = "karlPass", UserRole = UserRoleEnum.PowerUser, UserEmail = "Karl@gmail.com" },
                    new User() { UserID = 3, UserName = "Eric", UserPass = "ericPass", UserRole = UserRoleEnum.PowerUser, UserEmail = "Eric@gmail.com" },
                    new User() { UserID = 4, UserName = "Jorge", UserPass = "jorgePass", UserRole = UserRoleEnum.Guest, UserEmail = "Jorge@gmail.com" },
                    new User() { UserID = 5, UserName = "Ann", UserPass = "annPass", UserRole = UserRoleEnum.User, UserEmail = "Ann@gmail.com" }  ,
                    new User() { UserID = 6, UserName = "Annette", UserPass = "annPass", UserRole = UserRoleEnum.User, UserEmail = "Annette@gmail.com" }
              );
        }
    }
}
