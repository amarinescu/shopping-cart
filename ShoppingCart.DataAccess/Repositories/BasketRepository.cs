using ShoppingCart.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.DataAccess.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private List<Basket> _basketList;

        public BasketRepository()
        {
            _basketList = new List<Basket>();

            _basketList.Add(new Basket
            {
                BasketId = 1,
                Customer = "Alex",
                PaysVAT = true,
                Articles = new List<Article>
                {
                    new Article {  Item = "tomato", Price = 10},
                    new Article {  Item = "cucumbre", Price = 5}
                }
            });

            _basketList.Add(new Basket
            {
                BasketId = 2,
                Customer = "Andrei",
                PaysVAT = true,
                Articles = new List<Article>
                {
                    new Article {  Item = "pasta", Price = 15},
                    new Article {  Item = "marinara sauce", Price = 25}
                }
            });
        }

        public async Task<Basket> AddArticleToBasket(int basketId, Article article)
        {
            var targetBasket = _basketList.First(b => b.BasketId == basketId);

            if (targetBasket.Articles == null)
                targetBasket.Articles = new List<Article>();

            targetBasket.Articles.Add(article);

            return targetBasket;
        }

        public async Task<Basket> AddBasket(string customer, bool paysVAT)
        {
            var newBasketId = _basketList.OrderByDescending(b => b.BasketId).Select(b => b.BasketId).First() + 1;

            var newBasket = new Basket
            {
                BasketId = newBasketId,
                Customer = customer,
                PaysVAT = paysVAT,
            };

            _basketList.Add(newBasket);

            return newBasket;
        }

        public async Task CloseBasket(int basketId, bool payed)
        {
            var targetBasket = _basketList.First(b => b.BasketId == basketId);

            targetBasket.IsClosed = true;
            targetBasket.IsPayed = payed;
        }

        public async Task<Basket> GetBasketById(int basketId)
        {
            return _basketList.FirstOrDefault(b => b.BasketId == basketId);
        }
    }
}
