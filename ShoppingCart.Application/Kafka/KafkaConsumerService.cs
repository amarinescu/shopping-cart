using Confluent.Kafka;
using Confluent.Kafka.Admin;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShoppingCart.Application.Kafka.Commands;
using ShoppingCart.DataAccess.Context;
using ShoppingCart.DataAccess.Entities;
using ShoppingCart.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingCart.Application.Kafka
{
    public class KafkaConsumerService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public KafkaConsumerService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await InitKafka();
            await LaunchKafkaConsumerInBackground();
        }

        private static async Task InitKafka()
        {
            using (var adminClient = new AdminClientBuilder(KafkaCloudConfig.GetConfig()).Build())
            {
                try
                {
                    await adminClient.CreateTopicsAsync(new List<TopicSpecification> {
                        new TopicSpecification { Name = "shopping-cart", NumPartitions = 1, ReplicationFactor = 3 } });
                }
                catch (CreateTopicsException e)
                {
                    if (e.Results[0].Error.Code == ErrorCode.TopicAlreadyExists)
                        Console.WriteLine("Topic already exists"); //todo: replace with logs
                    else
                        throw;
                }
            }
        }

        private async Task LaunchKafkaConsumerInBackground()
        {
            var consumerConfig = new ConsumerConfig(KafkaCloudConfig.GetConfig());
            consumerConfig.GroupId = "shopping-cart-group-1";
            consumerConfig.AutoOffsetReset = AutoOffsetReset.Earliest;
            consumerConfig.EnableAutoCommit = true;

            using (var consumer = new ConsumerBuilder<string, string>(consumerConfig).Build())
            {
                consumer.Subscribe("shopping-cart");

                try
                {
                    while (true)
                    {
                        var consumerResult = consumer.Consume();
                        var ev = JsonSerializer.Deserialize<Event>(consumerResult.Message.Value);
                        
                        //simulating eventual consistency
                        Thread.Sleep(10000);

                        using (var scope = _serviceScopeFactory.CreateScope())
                        {
                            var mediator = scope.ServiceProvider.GetService<IMediator>();

                            switch (ev.EventType)
                            {
                                case EventType.BasketAdded:
                                    await mediator.Send(new HandleBasketAddedCommand(ev));
                                    break;
                                case EventType.BasketItemAdded:
                                    await mediator.Send(new HandleBasketItemAddedCommand(ev));
                                    break;
                                case EventType.BasketClosed:
                                    await mediator.Send(new HandleBasketClosedCommand(ev));
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                finally
                {
                    consumer.Close();
                }
            }
        }
    }
}
