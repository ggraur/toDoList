using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.Models
{
    public interface ICustomDbContextFactory<out T> where T : DbContext
    {
        T CreateDbContext(string connectionString);
    }
}
