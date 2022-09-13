using ShoppingCart.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.DataAccess.Repositories
{
    public interface IEventRepository
    {
        Task PersistEvent(Event ev);
    }
}
