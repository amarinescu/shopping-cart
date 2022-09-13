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
    public class HandleBasketClosedCommand : IRequest<Unit>
    {
        public Event Ev { get; set; }

        public HandleBasketClosedCommand(Event ev)
        {
            Ev = ev;
        }

        public class Handler : IRequestHandler<HandleBasketClosedCommand, Unit>
        {
            private readonly IBasketRepository _basketRepository;

            public Handler(IBasketRepository basketRepository)
            {
                _basketRepository = basketRepository;
            }

            public async Task<Unit> Handle(HandleBasketClosedCommand request, CancellationToken cancellationToken)
            {
                var basketCode = request.Ev.EntityIdentifier;
                var isPayed = JsonSerializer.Deserialize<bool>(request.Ev.Entity);

                await _basketRepository.CloseBasket(basketCode, isPayed);               

                return Unit.Value;
            }
        }
    }
}
