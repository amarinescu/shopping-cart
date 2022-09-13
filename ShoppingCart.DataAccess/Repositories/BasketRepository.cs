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

        public async Task<Basket> AddArticleToBasket(Guid basketCode, Article article)
        {
            var targetBasket = await _context.Baskets.Include(basket => basket.Articles).FirstAsync(b => b.BasketCode == basketCode);

            if (targetBasket.Articles == null)
                targetBasket.Articles = new List<Article>();

            targetBasket.Articles.Add(article);

            await _context.SaveChangesAsync();

            return targetBasket;
        }

        public async Task AddBasket(Basket basket)
        {
            _context.Baskets.Add(basket);

            await _context.SaveChangesAsync();
        }

        public async Task CloseBasket(Guid basketCode, bool payed)
        {
            var targetBasket = await _context.Baskets.FirstAsync(b => b.BasketCode == basketCode);

            targetBasket.IsClosed = true;
            targetBasket.IsPayed = payed;

            await _context.SaveChangesAsync();
        }

        public async Task<Basket> GetBasketByBasketCode(Guid basketCode)
        {
            return await _context.Baskets.Include(basket => basket.Articles).FirstOrDefaultAsync(b => b.BasketCode == basketCode);
        }
    }
}
