using MediatR;
using ShoppingCart.Application.Baskets.Models;
using ShoppingCart.Application.ErrorHandling;
using ShoppingCart.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingCart.Application.Baskets.Queries
{
    public class GetBasketByIdQuery : IRequest<Basket>
    {
        public int BasketId { get; set; }

        public class Handler : IRequestHandler<GetBasketByIdQuery, Basket>
        {
            private readonly IBasketRepository _basketRepository;

            public Handler(IBasketRepository basketRepository)
            {
                _basketRepository = basketRepository;
            }

            public async Task<Basket> Handle(GetBasketByIdQuery request, CancellationToken cancellationToken)
            {
                var basket = await _basketRepository.GetBasketById(request.BasketId);

                if(basket == null)
                    throw new BusinessException($"The basket with id {request.BasketId} was not found.", System.Net.HttpStatusCode.NotFound);

                return Basket.FromEntity(basket);
            }
        }
    }
}
