using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DD.OnlineNote.Model;

namespace DD.OnlineNote.DataLayer
{
    public interface IUserRepository
    {
        User Create(User user);
        void Delete(Guid id);
        User Get(Guid id);
        bool CheckUserByName(string Name);
    }
}
