using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.DataAccess.Entities
{
    public class Event
    {
        public int EventId { get; set; }
        public EventType EventType { get; set; }
        public Guid EntityIdentifier { get; set; }
        public string Entity { get; set; }
    }

    public enum EventType
    {
        BasketAdded,
        BasketItemAdded,
        BasketClosed
    }
}
