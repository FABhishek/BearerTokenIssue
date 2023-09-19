using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOTI.Project.DAL
{
    public interface IAccount
    {
         Task<User> ValidateUserAsync(string username, string password);
    }
}
