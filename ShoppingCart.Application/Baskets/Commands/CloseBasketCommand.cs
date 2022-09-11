using MediatR;
using ShoppingCart.Application.Baskets.Models;
using ShoppingCart.Application.ErrorHandling;
using ShoppingCart.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingCart.Application.Baskets.Commands
{
    public class CloseBasketCommand : IRequest<Unit>
    {
        private int BasketId { get; set; }

        public void AppendBasketId(int basketId)
        {
            BasketId = basketId;
        }

        public bool IsClosed { get; set; }
        public bool IsPayed { get; set; }

        public class Handler : IRequestHandler<CloseBasketCommand, Unit>
        {
            private readonly IBasketRepository _basketRepository;

            public Handler(IBasketRepository basketRepository)
            {
                _basketRepository = basketRepository;
            }

            public async Task<Unit> Handle(CloseBasketCommand request, CancellationToken cancellationToken)
            {
                var basket = await _basketRepository.GetBasketById(request.BasketId);

                if (basket == null)
                    throw new BusinessException($"The basket with id {request.BasketId} was not found.", System.Net.HttpStatusCode.NotFound);

                await _basketRepository.CloseBasket(request.BasketId, request.IsPayed);

                return Unit.Value;
            }
        }
    }
}
