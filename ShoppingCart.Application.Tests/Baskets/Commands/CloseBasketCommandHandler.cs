using Moq;
using ShoppingCart.Application.Baskets.Commands;
using ShoppingCart.Application.Kafka;
using ShoppingCart.DataAccess.Entities;
using ShoppingCart.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShoppingCart.Application.Tests.Baskets.Commands
{
    public class CloseBasketCommandHandler
    {
        [Fact]
        public async Task Handle_HappyPath_EndsWithSuccess()
        {
            //Arrange
            var request = new CloseBasketCommand()
            {
                IsClosed = true,
                IsPayed = true
            };
            var basket = new Basket()
            {
                BasketId = 1,
                BasketCode = Guid.NewGuid(),
                Customer = "Alex",
                PaysVAT = true,
                Articles = null
            };
            request.AppendBasketCode(basket.BasketCode);
            var mockBasketRepository = new Mock<IBasketRepository>();
            mockBasketRepository.Setup(x => x.GetBasketByBasketCode(basket.BasketCode)).ReturnsAsync(() => basket).Verifiable();

            var ev = new Event
            {
                EventType = EventType.BasketItemAdded,
                EntityIdentifier = basket.BasketCode,
                Entity = JsonSerializer.Serialize(request.IsPayed)
            };
            var mockEventRepository = new Mock<IEventRepository>();
            mockEventRepository.Setup(x => x.PersistEvent(ev)).Verifiable();
            var mockKafkaProducer = new Mock<IKafkaProducer>();
            mockKafkaProducer.Setup(x => x.Produce(ev)).Verifiable();

            var handler = new CloseBasketCommand.Handler(mockBasketRepository.Object, mockEventRepository.Object, mockKafkaProducer.Object);

            //Act
            await handler.Handle(request, CancellationToken.None);

            //Assert
            mockBasketRepository.Verify(x => x.GetBasketByBasketCode(basket.BasketCode), Times.Once);
            mockEventRepository.Verify(x => x.PersistEvent(It.IsAny<Event>()), Times.Once);
            mockKafkaProducer.Verify(x => x.Produce(It.IsAny<Event>()), Times.Once);
        }
    }
}
