using Microsoft.Extensions.Logging;
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
        private readonly ILogger<EventRepository> _logger;

        public EventRepository(ShoppingCartContext context, ILogger<EventRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task PersistEvent(Event ev)
        {
            _context.Events.Add(ev);

            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully persisted {EventType} event.", ev.EventType);
        }
    }
}
