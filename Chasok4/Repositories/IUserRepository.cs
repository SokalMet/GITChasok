
using Chasok4.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chasok4.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<AppUser> GetUsers();
        AppUser GetUserById(string userId);
        void InsertUser(AppUser user);
        void DeleteUser(string userId);
        void UpdateUser(AppUser user);
        void Save();
    }
}
