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
    public class AddArticleToBasketCommand : IRequest<Basket>
    {
        private int BasketId { get; set; }

        public void AppendBasketId(int basketId)
        {
            BasketId = basketId;
        }

        public string Item { get; set; }
        public decimal Price { get; set; }

        public class Handler : IRequestHandler<AddArticleToBasketCommand, Basket>
        {
            private readonly IBasketRepository _basketRepository;

            public Handler(IBasketRepository basketRepository)
            {
                _basketRepository = basketRepository;
            }

            public async Task<Basket> Handle(AddArticleToBasketCommand request, CancellationToken cancellationToken)
            {
                var basket = await _basketRepository.GetBasketById(request.BasketId);

                if (basket == null)
                    throw new BusinessException($"The basket with id {request.BasketId} was not found.", System.Net.HttpStatusCode.NotFound);

                var returnBasket = await _basketRepository.AddArticleToBasket(request.BasketId, new DataAccess.Entities.Article { Item = request.Item, Price = request.Price });

                return Basket.FromEntity(returnBasket);
            }
        }
    }
}
