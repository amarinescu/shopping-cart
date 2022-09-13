using Moq;
using ShoppingCart.Application.Baskets.Commands;
using ShoppingCart.Application.Kafka;
using ShoppingCart.DataAccess.Entities;
using ShoppingCart.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Application.Tests.Baskets.Commands
{
    public class AddBasketCommandHandlerTests
    {
        [Fact]
        public async Task Handle_HappyPath_EndsWithSuccess()
        {
            //Arrange
            var request = new AddBasketCommand
            {
                Customer = "Alex",
                PaysVAT = true
            };
            var mockEventRepository = new Mock<IEventRepository>();
            mockEventRepository.Setup(x => x.PersistEvent(It.IsAny<Event>())).Verifiable();
            var mockKafkaProducer = new Mock<IKafkaProducer>();
            mockKafkaProducer.Setup(x => x.Produce(It.IsAny<Event>())).Verifiable();

            var handler = new AddBasketCommand.Handler(mockEventRepository.Object, mockKafkaProducer.Object);
            
            //Act
            await handler.Handle(request, CancellationToken.None);

            //Assert
            mockEventRepository.Verify(x => x.PersistEvent(It.IsAny<Event>()), Times.Once);
            mockKafkaProducer.Verify(x => x.Produce(It.IsAny<Event>()), Times.Once);
        }
    }
}
