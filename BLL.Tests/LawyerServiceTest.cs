using Abstraction;
using Common;
using Moq;
using Xunit;

namespace BLL.Tests
{
    public class LawyerServiceTests
    {
        [Fact]
        public void GetAll_ShouldReturnAllLawyers_MappedToDto()
        {
            var dalLawyers = new List<Lawyer>
            {
                new Lawyer("Гарві", "Спектер", "L1", "Pearson Specter"),
                new Lawyer("Майк", "Росс", "L2", "Pearson Specter")
            };
            var mockContext = new Mock<IEntityContext<Lawyer>>();
            mockContext.Setup(c => c.GetAll()).Returns(dalLawyers);
            var service = new LawyerService(mockContext.Object);

            var result = service.GetAll();

            Assert.Equal(2, result.Count);
            Assert.Equal("Спектер", result[0].LastName);
        }

        [Fact]
        public void Remove_WhenPersonExists_ShouldCallContextSaveAll()
        {
            var lawyerToRemove = new Lawyer("Майк", "Росс", "L2", "Pearson Specter");
            var entities = new List<Lawyer> { new Lawyer("Гарві", "Спектер", "L1", "Pearson Specter"), lawyerToRemove };
            var mockContext = new Mock<IEntityContext<Lawyer>>();
            mockContext.Setup(c => c.GetAll()).Returns(entities);
            var service = new LawyerService(mockContext.Object);

            service.Remove("Майк", "Росс", "L2");

            mockContext.Verify(c => c.SaveAll(It.Is<List<Lawyer>>(l => l.Count == 1)), Times.Once);
        }

        [Fact]
        public void Practice_ShouldReturnCorrectMessage()
        {
            var mockContext = new Mock<IEntityContext<Lawyer>>();
            var service = new LawyerService(mockContext.Object);
            var dto = new LawyerDto { FirstName = "Джессіка", LastName = "Пірсон", Company = "PS" };

            var result = service.Practice(dto);

            Assert.Contains("готує судові документи та веде переговори", result);
        }

        [Fact]
        public void Find_WhenLawyerNotFound_ShouldThrowBusinessLogicException()
        {
            var mockContext = new Mock<IEntityContext<Lawyer>>();
            mockContext.Setup(c => c.GetAll()).Returns(new List<Lawyer>());
            var service = new LawyerService(mockContext.Object);

            var exception = Assert.Throws<BusinessLogicException>(
                () => service.Find("Неіснуючий", "Адвокат", "000"));

            Assert.Contains("Person not found.", exception.Message);
        }

        [Fact]
        public void GetAll_WhenDALThrowsException_ShouldThrowBusinessLogicException()
        {
            var mockContext = new Mock<IEntityContext<Lawyer>>();
            mockContext.Setup(c => c.GetAll()).Throws(new InvalidOperationException());
            var service = new LawyerService(mockContext.Object);

            var exception = Assert.Throws<BusinessLogicException>(() => service.GetAll());

            Assert.Contains("Error retrieving entities from data layer.", exception.Message);
            Assert.IsType<InvalidOperationException>(exception.InnerException);
        }
    }
}