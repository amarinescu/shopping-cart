using ShoppingCart.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.DataAccess.Repositories
{
    public interface IBasketRepository
    {
        Task<Basket> AddBasket(string customer, bool paysVAT);
        Task<Basket> AddArticleToBasket(int basketId, Article article);
        Task<Basket> GetBasketById(int basketId);
        Task CloseBasket(int basketId, bool payed);
    }
}
