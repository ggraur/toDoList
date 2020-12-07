using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.Models
{
    public interface IUserAddress
    {
        public AppUserAddress Add(AppUserAddress userAddress);
        public AppUserAddress Update(AppUserAddress userAddress);
        public AppUserAddress Delete(AppUserAddress userAddress);
        public IEnumerable<AppUserAddress> GetUserAddress(int UserAddressID);

    }
}
