using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ShoppingCart.DataAccess.Context;
using ShoppingCart.DataAccess.Entities;
using ShoppingCart.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace ShoppingCart.DataAccess.Tests.Repositories
{
    public class EventRepositoryTests
    {
        private DbContextOptions<ShoppingCartContext> dbContextOptions;
        private ShoppingCartContext? _mockShoppingCartContext;

        public EventRepositoryTests()
        {
            var dbName = $"ShoppingCartTestDB_{DateTime.Now}";
            dbContextOptions = new DbContextOptionsBuilder<ShoppingCartContext>().UseInMemoryDatabase(dbName).Options;
        }

        [Fact]
        public async Task PersistEvent_HappyPath_EndsWithSuccess()
        {
            //Arrange
            var expectedEvent = new Event
            {
                EventId = 1,
                EventType = EventType.BasketAdded,
                EntityIdentifier = Guid.NewGuid(),
                Entity = JsonSerializer.Serialize("test")
            };
            _mockShoppingCartContext = new ShoppingCartContext(dbContextOptions);
            var mockLogger = new Mock<ILogger<EventRepository>>();
            var eventRepository = new EventRepository(_mockShoppingCartContext, mockLogger.Object);

            //Act
            await eventRepository.PersistEvent(expectedEvent);

            //Assert
            var actualEvent = await _mockShoppingCartContext.Events.FirstOrDefaultAsync();
            Assert.NotNull(actualEvent);
            Assert.Equal(expectedEvent.EventId, actualEvent.EventId);
            Assert.Equal(expectedEvent.EventType, actualEvent.EventType);
            Assert.Equal(expectedEvent.EntityIdentifier, actualEvent.EntityIdentifier);
            Assert.Equal(JsonSerializer.Deserialize<string>(expectedEvent.Entity), JsonSerializer.Deserialize<string>(actualEvent.Entity));

            mockLogger.Verify(x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }
    }
}
