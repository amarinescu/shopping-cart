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
    public class HandleBasketAddedCommand : IRequest<Unit>
    {
        public Event Ev { get; set; }

        public HandleBasketAddedCommand(Event ev)
        {
            Ev = ev;
        }

        public class Handler : IRequestHandler<HandleBasketAddedCommand, Unit>
        {
            private readonly IBasketRepository _basketRepository;

            public Handler(IBasketRepository basketRepository)
            {
                _basketRepository = basketRepository;
            }

            public async Task<Unit> Handle(HandleBasketAddedCommand request, CancellationToken cancellationToken)
            {
                var basket = JsonSerializer.Deserialize<Basket>(request.Ev.Entity);

                await _basketRepository.AddBasket(basket);

                return Unit.Value;
            }
        }
    }
}
