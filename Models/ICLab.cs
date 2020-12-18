using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.Models
{
    public interface ICLab
    {
        public int Create(CLab cLab);
        public Task<bool> InsertAsync(CLab cLab);
        public Task<bool> DeleteAsync(CLab cLab);
    }
}
