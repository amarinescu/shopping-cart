using Microsoft.EntityFrameworkCore;
using ShoppingCart.DataAccess.Context;
using ShoppingCart.DataAccess.Entities;
using ShoppingCart.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.DataAccess.Tests.Repositories
{
    public class BasketRepositoryTests
    {
        private DbContextOptions<ShoppingCartContext> dbContextOptions;
        private ShoppingCartContext? _mockShoppingCartContext;

        public BasketRepositoryTests()
        {
            var dbName = $"ShoppingCartTestDB_{DateTime.Now}";
            dbContextOptions = new DbContextOptionsBuilder<ShoppingCartContext>().UseInMemoryDatabase(dbName).Options;
        }

        [Fact]
        public async Task AddArticleToBasket_HappyPath_EndsWithSuccess()
        {
            //Arrange
            var expectedArticle = new Article
            {
                Item = "frozen pizza",
                Price = 40
            };
            var basketCode = Guid.Parse("327a116c-3120-436d-9780-2c78773c5416");
            var basketRepository = await CreateRepositoryAsync();


            //Act
            await basketRepository.AddArticleToBasket(basketCode, expectedArticle);

            //Assert
            var actualBasket = _mockShoppingCartContext.Baskets.First(b => b.BasketCode == basketCode);
            Assert.Equal(3, actualBasket.Articles.Count);
            Assert.Equal(7, actualBasket.Articles.Last().ArticleId);
            Assert.Equal(expectedArticle.Item, actualBasket.Articles.Last().Item);
            Assert.Equal(expectedArticle.Price, actualBasket.Articles.Last().Price);

        }

        [Fact]
        public async Task AddBasket_HappyPath_EndsWithSuccess()
        {
            //Arrange
            var expectedBasket = new Basket
            {
                BasketCode = Guid.NewGuid(),
                Customer = "Robert",
                PaysVAT = true
            };
            var basketRepository = await CreateRepositoryAsync();

            //Act
            await basketRepository.AddBasket(expectedBasket);

            //Assert
            var actualBasket = _mockShoppingCartContext.Baskets.OrderByDescending(b => b.BasketId).First();
            Assert.Equal(expectedBasket.BasketCode, actualBasket.BasketCode);
            Assert.Equal(expectedBasket.Customer, actualBasket.Customer);
            Assert.Equal(expectedBasket.PaysVAT, actualBasket.PaysVAT);
        }

        [Fact]
        public async Task CloseBasket_HappyPath_EndsWithSuccess()
        {
            //Arrange
            var basketCode = Guid.Parse("327a116c-3120-436d-9780-2c78773c5416");
            var basketRepository = await CreateRepositoryAsync();

            //Act
            await basketRepository.CloseBasket(basketCode, true);

            //Assert
            var actualBasket = _mockShoppingCartContext.Baskets.First(b => b.BasketCode == basketCode);
            Assert.True(actualBasket.IsClosed);
            Assert.True(actualBasket.IsPayed);
        }

        [Fact]
        public async Task GetBasketByBasketCode_HappyPath_EndsWithSuccess()
        {
            //Arrange
            var basketRepository = await CreateRepositoryAsync();
            var expectedBasket = _mockShoppingCartContext.Baskets.First();

            //Act
            var actualBasket = await basketRepository.GetBasketByBasketCode(expectedBasket.BasketCode);

            //Asert
            Assert.Equal(expectedBasket.BasketCode, actualBasket.BasketCode);
            Assert.Equal(expectedBasket.Customer, actualBasket.Customer);
            Assert.Equal(expectedBasket.PaysVAT, actualBasket.PaysVAT);
        }

        #region private methods
        private async Task<BasketRepository> CreateRepositoryAsync()
        {
            _mockShoppingCartContext = new ShoppingCartContext(dbContextOptions);
            await PopulateDataAsync(_mockShoppingCartContext);
            return new BasketRepository(_mockShoppingCartContext);
        }

        private async Task PopulateDataAsync(ShoppingCartContext context)
        {
            var basket = new Basket
            {
                BasketCode = Guid.Parse("43ee881b-4a07-4a78-8d71-80c3d8169341"),
                Customer = "Alex",
                PaysVAT = true,
                Articles = new List<Article>
                {
                    new Article { Item = "pasta", Price = 10},
                    new Article { Item = "marinara sauce", Price = 25}
                }
            };
            await context.Baskets.AddAsync(basket);

            basket = new Basket
            {
                BasketCode = Guid.Parse("702ba94e-432d-4533-8a3c-29b006a16973"),
                Customer = "Maria",
                PaysVAT = true,
                Articles = new List<Article>
                {
                    new Article { Item = "chocolate", Price = 12},
                    new Article { Item = "ice cream", Price = 35}
                }
            };
            await context.Baskets.AddAsync(basket);

            basket = new Basket
            {
                BasketCode = Guid.Parse("327a116c-3120-436d-9780-2c78773c5416"),
                Customer = "Tudor",
                PaysVAT = true,
                Articles = new List<Article>
                {
                    new Article { Item = "beer", Price = 25},
                    new Article { Item = "potatoe chips", Price = 15}
                }
            };
            await context.Baskets.AddAsync(basket);

            await context.SaveChangesAsync();
        }
        #endregion
    }
}
