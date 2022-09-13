using ShoppingCart.DataAccess.Context;
using ShoppingCart.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.DataAccess.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly ShoppingCartContext _context;

        public EventRepository(ShoppingCartContext context)
        {
            _context = context;
        }

        public async Task PersistEvent(Event ev)
        {
            _context.Events.Add(ev);

            await _context.SaveChangesAsync();
        }
    }
}
