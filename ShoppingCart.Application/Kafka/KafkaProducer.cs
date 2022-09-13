using Confluent.Kafka;
using ShoppingCart.Application.ErrorHandling;
using ShoppingCart.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShoppingCart.Application.Kafka
{
    public class KafkaProducer : IKafkaProducer
    {
        public async Task Produce(Event ev)
        {
            using (var producer = new ProducerBuilder<string, string>(KafkaCloudConfig.GetConfig()).Build())
            {
                var key = Guid.NewGuid().ToString();
                var val = JsonSerializer.Serialize(ev);

                await producer.ProduceAsync("shopping-cart", new Message<string, string> { Key = key, Value = val });

                producer.Flush(TimeSpan.FromSeconds(10));
            }
        }
    }
}
