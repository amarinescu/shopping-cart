using MediatR;
using ShoppingCart.Application.Baskets.Models;
using ShoppingCart.Application.Kafka;
using ShoppingCart.DataAccess.Entities;
using ShoppingCart.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingCart.Application.Baskets.Commands
{
    public class AddBasketCommand : IRequest<Guid>
    {
        public string Customer { get; set; }
        public bool PaysVAT { get; set; }

        public class Handler : IRequestHandler<AddBasketCommand, Guid>
        {
            private readonly IEventRepository _eventRepository;
            private readonly IKafkaProducer _kafkaProducer;

            public Handler(IEventRepository eventRepository, IKafkaProducer kafkaProducer)
            {
                _eventRepository = eventRepository;
                _kafkaProducer = kafkaProducer;
            }

            public async Task<Guid> Handle(AddBasketCommand request, CancellationToken cancellationToken)
            {
                var basketCode = Guid.NewGuid();

                var basket = new DataAccess.Entities.Basket
                {
                    Customer = request.Customer,
                    PaysVAT = request.PaysVAT,
                    BasketCode = basketCode
                };

                var ev = new Event
                {
                    EventType = EventType.BasketAdded,
                    EntityIdentifier = basketCode,
                    Entity = JsonSerializer.Serialize(basket)
                };

                await _eventRepository.PersistEvent(ev);
                await _kafkaProducer.Produce(ev);

                return basketCode;
            }
        }
    }
}
