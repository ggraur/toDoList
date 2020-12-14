using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoClassLibrary;
using toDoList.ViewModels;

namespace toDoList.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<MyUser>().HasData(
            //        new MyUser() { UserID = 1, UserName = "Mary", UserPass = "maryPass", UserRole = UserRoleEnum.Administrator, UserEmail = "mary@gmail.com" },
            //        new MyUser() { UserID = 2, UserName = "Karl", UserPass = "karlPass", UserRole = UserRoleEnum.PowerUser, UserEmail = "Karl@gmail.com" },
            //        new MyUser() { UserID = 3, UserName = "Eric", UserPass = "ericPass", UserRole = UserRoleEnum.PowerUser, UserEmail = "Eric@gmail.com" },
            //        new MyUser() { UserID = 4, UserName = "Jorge", UserPass = "jorgePass", UserRole = UserRoleEnum.Guest, UserEmail = "Jorge@gmail.com" },
            //        new MyUser() { UserID = 5, UserName = "Ann", UserPass = "annPass", UserRole = UserRoleEnum.User, UserEmail = "Ann@gmail.com" }  ,
            //        new MyUser() { UserID = 6, UserName = "Annette", UserPass = "annPass", UserRole = UserRoleEnum.User, UserEmail = "Annette@gmail.com" }
            //  );

            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser()
                {
                    Id = "82b1a0e1-61f5-40ab-9773-bb74814413f8",
                    UserName = "superadmin@gmail.com",
                    NormalizedUserName = "SUPERADMIN@GMAIL.COM",
                    Email = "superadmin@gmail.com",
                    NormalizedEmail = "SUPERADMIN@GMAIL.COM",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAEAACcQAAAAEEgjKSeD2Ds8RIJ40d4OYKamq5wQopldzWzPJicN6IHgC/ZrMFjZyvyUX+Tuqdu1LQ==",
                    SecurityStamp = "V4PJL2RQEZ6FIRGR3LQPLMKQVYEQIMIL",
                    ConcurrencyStamp = "6a985bc3-e1c5-4b3e-80a4-7ac93ce7030a" 
                }

             //new MyUser() { UserID = 1, UserName = "Mary", UserPass = "maryPass", UserRole = UserRoleEnum.Administrator, UserEmail = "mary@gmail.com" },
             //new MyUser() { UserID = 2, UserName = "Karl", UserPass = "karlPass", UserRole = UserRoleEnum.PowerUser, UserEmail = "Karl@gmail.com" },
             //new MyUser() { UserID = 3, UserName = "Eric", UserPass = "ericPass", UserRole = UserRoleEnum.PowerUser, UserEmail = "Eric@gmail.com" },
             //new MyUser() { UserID = 4, UserName = "Jorge", UserPass = "jorgePass", UserRole = UserRoleEnum.Guest, UserEmail = "Jorge@gmail.com" },
             //new MyUser() { UserID = 5, UserName = "Ann", UserPass = "annPass", UserRole = UserRoleEnum.User, UserEmail = "Ann@gmail.com" },
             //new MyUser() { UserID = 6, UserName = "Annette", UserPass = "annPass", UserRole = UserRoleEnum.User, UserEmail = "Annette@gmail.com" }
             );

            modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole() { Id = "1fa0c9b8-45f9-4835-9a3e-a6eb2b64d005", Name = "Super Admin",   NormalizedName = "SUPER ADMIN",   ConcurrencyStamp = "2134c8a2-6538-453d-8726-4f6b9e21d3f5" },
            new IdentityRole() { Id = "8db3a46b-918b-4b0f-90e9-81fa103f262e", Name = "Administrator", NormalizedName = "ADMINISTRATOR", ConcurrencyStamp = "fe4e795f-7624-4a96-b684-50c069396a24" },
            new IdentityRole() { Id = "bb413bf2-537e-497e-bf5c-68dc0002e47c", Name = "Power User",    NormalizedName = "POWER USER",    ConcurrencyStamp = "8a4b1f65-36e3-4a42-b3b2-d7f6d688ae14" },
            new IdentityRole() { Id = "fc2ab893-2d06-4f50-8608-a94bd4d3ab3a", Name = "User",          NormalizedName = "USER",          ConcurrencyStamp = "eb5ecd96-0446-4342-af10-8f47aef49fa4" });
            
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>() {RoleId= "1fa0c9b8-45f9-4835-9a3e-a6eb2b64d005", UserId= "82b1a0e1-61f5-40ab-9773-bb74814413f8"});

            //aqui pode ser verificacao se a view exist ...
        }
    }
}
