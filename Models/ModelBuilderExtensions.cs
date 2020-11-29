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
        public static void Seed(this ModelBuilder modelBuilder) {
            modelBuilder.Entity<User>().HasData(
                   new User() { UserID = 1, UserName = "Mary", UserPass = "maryPass", UserRole = UserRoleEnum.Administrator, UserEmail = "mary@gmail.com" }
              );
        }
    }
}
