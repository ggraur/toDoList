using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.Models
{
    public class SQL_ICLab : ICLab
    {
        public readonly AppDbContext dbContext;
        private readonly ILogger<SQL_ICLab> logger;
        public SQL_ICLab(AppDbContext _dbContext, ILogger<SQL_ICLab> _logger)
        {
            this.dbContext = _dbContext;
            this.logger = _logger;
        }

        public int Create(CLab cLab)
        {
            dbContext.cLabs.Add(cLab);
            return dbContext.SaveChanges();
        }

        public async Task<bool> DeleteAsync(CLab cLab)
        {
            bool bResult = false;
            if (cLab != null)
            {
                try
                {
                    dbContext.cLabs.Remove(cLab);
                    var result = await dbContext.SaveChangesAsync();
                    bResult = true;
                }
                catch (DbUpdateException ex)
                {
                    logger.Log(LogLevel.Warning, ex.Message);
                    bResult = false;
                }
            }
            return bResult;
        }
        public async Task<bool> InsertAsync(CLab cLab)
        {
            bool bResult = false;
            if (cLab != null)
            {
                try
                {
                    dbContext.cLabs.Add(cLab);
                    int iRresult = await dbContext.SaveChangesAsync();
                    return (iRresult > 0) ? true : false;
                }
                catch (DbUpdateException ex)
                {
                    logger.Log(LogLevel.Warning, ex.Message);
                    bResult = false;
                }
            }
            return bResult;
        }
    }
}
