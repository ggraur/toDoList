using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace toDoList.SystemDAL
{
    public class DataContext:DbContext
    {
        public DataContext(string connectionString) : base(GetOptions(connectionString))
        {
            
             
        }
        private static DbContextOptions GetOptions(string connectionString)
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
        }

        //public DbSet<Job> Jobs { get; set; }
    }
}

//https://www.iditect.com/how-to/54261817.html
//https://www.codeproject.com/Articles/848111/Multi-Tenancy-System-With-Separate-Databases-in-MV
//scafolding
//https://docs.microsoft.com/en-us/ef/core/cli/dotnet

/*
 *    call in program
 *    
 [Authorize]
public ActionResult Index()
{
   // get the current user:
   var accountContext = new AccountDAL.DataContext();
   var user = accountContext.Users.FirstOrDefault(x => x.Email == User.Identity.Name);

   if (user != null)
   {
      // now we have the current user, we can use their Account 
      // to create a new DataContext to access system data:
      var systemContext = new SystemDAL.DataContext(user.Account.Database);
      return View(systemContext.Jobs);
   }
   return View();
}
 */