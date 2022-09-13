using Moq;
using ShoppingCart.Application.Baskets.Queries;
using ShoppingCart.DataAccess.Entities;
using ShoppingCart.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Application.Tests.Baskets.Queries
{
    public class GetBasketByIdQueryHandlerTests
    {
        [Fact]
        public async Task Handle_HappyPath_EndsWithSuccess()
        {
            //Arrange
            var request = new GetBasketByIdQuery()
            {
                BasketCode = Guid.NewGuid()
            };

            var basket = new Basket()
            {
                BasketId = 1,
                BasketCode = request.BasketCode,
                Customer = "Alex",
                PaysVAT = true,
                IsClosed = false,
                IsPayed = false,
                Articles = new List<Article>
                {
                    new Article() { ArticleId = 1, Item = "tomatoes", Price = 10 },
                    new Article() { ArticleId = 2, Item = "cucumbres", Price = 5 }
                }
            };

            var mockBasketRepository = new Mock<IBasketRepository>();
            mockBasketRepository.Setup(x => x.GetBasketByBasketCode(basket.BasketCode)).ReturnsAsync(() => basket).Verifiable();

            var handler = new GetBasketByIdQuery.Handler(mockBasketRepository.Object);

            //Act
            var actualBasket = await handler.Handle(request, CancellationToken.None);

            //Assert
            Assert.NotNull(actualBasket);
            Assert.Equal(basket.BasketId, actualBasket.BasketId);
            Assert.Equal(basket.BasketCode, actualBasket.BasketCode);
            Assert.Equal(basket.Customer, actualBasket.Customer);
            Assert.Equal(basket.PaysVAT, actualBasket.PaysVAT);
            Assert.Equal(basket.IsClosed, actualBasket.IsClosed);
            Assert.Equal(basket.IsPayed, actualBasket.IsPayed);
            Assert.Equal(15, actualBasket.TotalNet);
            Assert.Equal(16.5m, actualBasket.TotalGross);

            mockBasketRepository.Verify(x => x.GetBasketByBasketCode(basket.BasketCode), Times.Once);
        }
    }
}
