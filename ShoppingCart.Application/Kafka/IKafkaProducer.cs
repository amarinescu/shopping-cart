using ShoppingCart.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Application.Kafka
{
    public interface IKafkaProducer
    {
        Task Produce(Event ev);
    }
}
