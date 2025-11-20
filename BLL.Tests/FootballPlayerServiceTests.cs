using Abstraction;
using Common;
using Moq;
using Xunit;

namespace BLL.Tests
{
    public class FootballPlayerServiceTests
    {
        [Fact]
        public void GetAll_ShouldReturnAllPlayers_MappedToDto()
        {
            var dalPlayers = new List<FootballPlayer>
            {
                new FootballPlayer("Андрій", "Шевченко", "P1", "Динамо"),
                new FootballPlayer("Ліонель", "Мессі", "P2", "Інтер Маямі")
            };
            var mockContext = new Mock<IEntityContext<FootballPlayer>>();
            mockContext.Setup(c => c.GetAll()).Returns(dalPlayers);
            var service = new FootballPlayerService(mockContext.Object);

            var result = service.GetAll();

            Assert.Equal(2, result.Count);
            Assert.Equal("Шевченко", result[0].LastName);
        }

        [Fact]
        public void Add_WhenValidDto_ShouldCallContextAdd()
        {
            var mockContext = new Mock<IEntityContext<FootballPlayer>>();
            var service = new FootballPlayerService(mockContext.Object);
            var dto = new FootballPlayerDto { FirstName = "Кріштіану", LastName = "Роналду", Passport = "P3", Team = "Аль-Наср" };

            service.Add(dto);

            mockContext.Verify(c => c.Add(It.IsAny<FootballPlayer>()), Times.Once);
        }

        [Fact]
        public void Practice_ShouldReturnCorrectMessage()
        {
            var mockContext = new Mock<IEntityContext<FootballPlayer>>();
            var service = new FootballPlayerService(mockContext.Object);
            var dto = new FootballPlayerDto { FirstName = "Кіліан", LastName = "Мбаппе", Team = "ПСЖ" };

            var result = service.Practice(dto);

            Assert.Contains("тренує швидкість", result);
        }

        [Fact]
        public void Find_WhenPlayerExists_ShouldReturnDto()
        {
            var player = new FootballPlayer("Андрій", "Шевченко", "P1", "Динамо");
            var mockContext = new Mock<IEntityContext<FootballPlayer>>();
            mockContext.Setup(c => c.GetAll()).Returns(new List<FootballPlayer> { player });
            var service = new FootballPlayerService(mockContext.Object);

            var result = service.Find("Андрій", "Шевченко", "P1");

            Assert.Equal("Динамо", result.Team);
        }

        [Fact]
        public void Remove_WhenPersonExists_ShouldCallContextSaveAll()
        {
            var playerToRemove = new FootballPlayer("Андрій", "Шевченко", "P1", "Динамо");
            var entities = new List<FootballPlayer> { playerToRemove, new FootballPlayer("Ліонель", "Мессі", "P2", "Інтер Маямі") };
            var mockContext = new Mock<IEntityContext<FootballPlayer>>();
            mockContext.Setup(c => c.GetAll()).Returns(entities);
            var service = new FootballPlayerService(mockContext.Object);

            service.Remove("Андрій", "Шевченко", "P1");

            mockContext.Verify(c => c.SaveAll(It.Is<List<FootballPlayer>>(l => l.Count == 1)), Times.Once);
        }
    }
}