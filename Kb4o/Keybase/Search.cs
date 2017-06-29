using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kb4o.Keybase
{
    public class Search : ISearchable
    {
        public ICollection<User> FindUser()
        {
            throw new NotImplementedException();
        }
    }


    public interface ISearchable
    {
        ICollection<User> FindUser();
    }
}
