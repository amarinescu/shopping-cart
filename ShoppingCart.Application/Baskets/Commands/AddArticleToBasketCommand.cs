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
    public class AddArticleToBasketCommand : IRequest<Unit>
    {
        private Guid BasketCode { get; set; }

        public void AppendBasketCode(Guid basketCode)
        {
            BasketCode = basketCode;
        }

        public string Item { get; set; }
        public decimal Price { get; set; }

        public class Handler : IRequestHandler<AddArticleToBasketCommand, Unit>
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

            public async Task<Unit> Handle(AddArticleToBasketCommand request, CancellationToken cancellationToken)
            {
                var basket = await _basketRepository.GetBasketByBasketCode(request.BasketCode);

                if (basket == null)
                    throw new BusinessException($"The basket with id {request.BasketCode} was not found.", System.Net.HttpStatusCode.NotFound);

                var article = new DataAccess.Entities.Article
                {
                    Item = request.Item,
                    Price = request.Price
                };

                var ev = new Event
                {
                    EventType = EventType.BasketItemAdded,
                    EntityIdentifier = basket.BasketCode,
                    Entity = JsonSerializer.Serialize(article)
                };

                await _eventRepository.PersistEvent(ev);
                await _kafkaProducer.Produce(ev);

                return Unit.Value;
            }
        }
    }
}
