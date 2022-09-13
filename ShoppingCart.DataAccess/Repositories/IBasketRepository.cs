using ShoppingCart.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.DataAccess.Repositories
{
    public interface IBasketRepository
    {
        Task AddBasket(Basket basket);
        Task<Basket> AddArticleToBasket(Guid basketCode, Article article);
        Task<Basket> GetBasketByBasketCode(Guid basketCode);
        Task CloseBasket(Guid basketCode, bool payed);
    }
}
