using MediatR;
using ShoppingCart.Application.Baskets.Models;
using ShoppingCart.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingCart.Application.Baskets.Commands
{
    public class AddBasketCommand : IRequest<Basket>
    {
        public string Customer { get; set; }
        public bool PaysVAT { get; set; }

        public class Handler : IRequestHandler<AddBasketCommand, Basket>
        {
            private readonly IBasketRepository _basketRepository;

            public Handler(IBasketRepository basketRepository)
            {
                _basketRepository = basketRepository;
            }

            public async Task<Basket> Handle(AddBasketCommand request, CancellationToken cancellationToken)
            {
                var basket = await _basketRepository.AddBasket(request.Customer, request.PaysVAT);

                return Basket.FromEntity(basket);
            }
        }
    }
}
