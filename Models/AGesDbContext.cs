using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.Models
{
    public class AGesDbContext   : DbContext
    {
        public AGesDbContext(DbContextOptions<AGesDbContext> options) : base(options)
        {

        }


    }
}

