using MediatR;
using ShoppingCart.DataAccess.Entities;
using ShoppingCart.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingCart.Application.Kafka.Commands
{
    public class HandleBasketItemAddedCommand : IRequest<Unit>
    {
        public Event Ev { get; set; }

        public HandleBasketItemAddedCommand(Event ev)
        {
            Ev = ev;
        }

        public class Handler : IRequestHandler<HandleBasketItemAddedCommand, Unit>
        {
            private readonly IBasketRepository _basketRepository;

            public Handler(IBasketRepository basketRepository)
            {
                _basketRepository = basketRepository;
            }

            public async Task<Unit> Handle(HandleBasketItemAddedCommand request, CancellationToken cancellationToken)
            {
                var basketId = request.Ev.EntityIdentifier;
                var article = JsonSerializer.Deserialize<Article>(request.Ev.Entity);

                await _basketRepository.AddArticleToBasket(basketId, article);

                return Unit.Value;
            }
        }
    }
}
