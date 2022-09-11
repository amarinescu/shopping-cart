using Microsoft.EntityFrameworkCore;
using ShoppingCart.DataAccess.Context;
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
        private readonly ShoppingCartContext _context;

        public BasketRepository(ShoppingCartContext context)
        {
            _context = context;
        }

        public async Task<Basket> AddArticleToBasket(int basketId, Article article)
        {
            var targetBasket = await _context.Baskets.Include(basket => basket.Articles).FirstAsync(b => b.BasketId == basketId);

            if (targetBasket.Articles == null)
                targetBasket.Articles = new List<Article>();

            targetBasket.Articles.Add(article);

            await _context.SaveChangesAsync();

            return targetBasket;
        }

        public async Task<Basket> AddBasket(string customer, bool paysVAT)
        {
            var newBasket = new Basket
            {
                Customer = customer,
                PaysVAT = paysVAT,
            };

            _context.Baskets.Add(newBasket);

            await _context.SaveChangesAsync();

            return newBasket;
        }

        public async Task CloseBasket(int basketId, bool payed)
        {
            var targetBasket = await _context.Baskets.FirstAsync(b => b.BasketId == basketId);

            targetBasket.IsClosed = true;
            targetBasket.IsPayed = payed;

            await _context.SaveChangesAsync();
        }

        public async Task<Basket> GetBasketById(int basketId)
        {
            return await _context.Baskets.Include(basket => basket.Articles).FirstOrDefaultAsync(b => b.BasketId == basketId);
        }
    }
}
