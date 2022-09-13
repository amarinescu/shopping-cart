using MediatR;
using ShoppingCart.Application.Baskets.Models;
using ShoppingCart.Application.ErrorHandling;
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
    public class CloseBasketCommand : IRequest<Unit>
    {
        private Guid BasketCode { get; set; }

        public void AppendBasketCode(Guid basketCode)
        {
            BasketCode = basketCode;
        }

        public bool IsClosed { get; set; }
        public bool IsPayed { get; set; }

        public class Handler : IRequestHandler<CloseBasketCommand, Unit>
        {
            private readonly IBasketRepository _basketRepository;
            private readonly IEventRepository _eventRepository;
            private readonly IKafkaProducer _kafkaProducer;

            public Handler(IBasketRepository basketRepository, IEventRepository eventRepository, IKafkaProducer kafkaProducer)
            {
                _basketRepository = basketRepository;
                _eventRepository = eventRepository;
                _kafkaProducer = kafkaProducer;
            }

            public async Task<Unit> Handle(CloseBasketCommand request, CancellationToken cancellationToken)
            {
                var basket = await _basketRepository.GetBasketByBasketCode(request.BasketCode);

                if (basket == null)
                    throw new BusinessException($"The basket with id {request.BasketCode} was not found.", System.Net.HttpStatusCode.NotFound);

                var ev = new Event
                {
                    EventType = EventType.BasketClosed,
                    EntityIdentifier = basket.BasketCode,
                    Entity = JsonSerializer.Serialize(request.IsPayed)
                };

                await _eventRepository.PersistEvent(ev);
                await _kafkaProducer.Produce(ev);

                return Unit.Value;
            }
        }
    }
}
