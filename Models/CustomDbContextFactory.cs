using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.Models
{
    //https://stackoverflow.com/questions/54760793/create-an-ef-core-dbcontext-at-runtime-based-on-request-parameters
    //https://docs.microsoft.com/en-us/ef/core/dbcontext-configuration/
    public class CustomDbContextFactory<T> : ICustomDbContextFactory<T> where T : DbContext
    {
        public T CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<T>();
            optionsBuilder.UseSqlServer(connectionString);
            return System.Activator.CreateInstance(typeof(T), optionsBuilder.Options) as T;
        }
    }
}
