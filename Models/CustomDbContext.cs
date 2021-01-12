using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.Models
{
    public class CustomDbContext :DbContext
    {
        private readonly string connectionString;
        //private readonly DbContextOptions dbContextOptions;

        //public CustomDbContext([NotNullAttribute]DbContextOptions _dbContextOptions):base(_dbContextOptions)
        //{
        //    this.dbContextOptions = _dbContextOptions;
        //}
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.Options = dbContextOptions;

        //    optionsBuilder.UseSqlServer(optionsBuilder.UseSqlServer() );
        //}

        public CustomDbContext(string _connectionString)
        {
            this.connectionString = _connectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
