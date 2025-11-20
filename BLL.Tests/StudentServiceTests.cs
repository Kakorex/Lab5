using Abstraction;
using Common;
using Moq;
using Xunit;

namespace BLL.Tests
{
    public class StudentServiceTests
    {
        [Fact]
        public void GetAll_ShouldReturnAllStudents_MappedToDto()
        {
            var dalStudents = new List<Student>
            {
                new Student("Іван", "Франко", "PF123456", "ID001", 3, "N/A"),
                new Student("Леся", "Українка", "PU654321", "ID002", 5, "Моб007")
            };
            var mockContext = new Mock<IEntityContext<Student>>();
            mockContext.Setup(c => c.GetAll()).Returns(dalStudents);
            var service = new StudentService(mockContext.Object);

            var result = service.GetAll();

            Assert.Equal(2, result.Count);
            mockContext.Verify(c => c.GetAll(), Times.Once);
            Assert.Equal("Іван", result[0].FirstName);
        }

        [Fact]
        public void Add_WhenValidDto_ShouldCallContextAdd()
        {
            var mockContext = new Mock<IEntityContext<Student>>();
            var service = new StudentService(mockContext.Object);
            var dto = new StudentDto { FirstName = "Valid", LastName = "Person", Passport = "P1", Course = 1 };

            service.Add(dto);

            mockContext.Verify(c => c.Add(It.IsAny<Student>()), Times.Once);
        }

        [Theory]
        [InlineData("", "Valid", "First name and last name must be specified.")]
        [InlineData("Valid", "", "First name and last name must be specified.")]
        public void Add_WhenNameIsInvalid_ShouldThrowBusinessLogicException_AndNotCallDAL(string firstName, string lastName, string expectedMessage)
        {
            var mockContext = new Mock<IEntityContext<Student>>();
            var service = new StudentService(mockContext.Object);
            var invalidDto = new StudentDto { FirstName = firstName, LastName = lastName };

            var exception = Assert.Throws<BusinessLogicException>(() => service.Add(invalidDto));

            Assert.Contains(expectedMessage, exception.Message);
            mockContext.Verify(c => c.Add(It.IsAny<Student>()), Times.Never);
        }

        [Fact]
        public void Add_WhenDALThrowsException_ShouldThrowBusinessLogicException()
        {
            var mockContext = new Mock<IEntityContext<Student>>();
            mockContext.Setup(c => c.Add(It.IsAny<Student>())).Throws(new InvalidOperationException());
            var service = new StudentService(mockContext.Object);
            var dto = new StudentDto { FirstName = "Valid", LastName = "Person", Passport = "P1", Course = 1 };

            var exception = Assert.Throws<BusinessLogicException>(() => service.Add(dto));

            Assert.Contains("Error adding entity to data layer.", exception.Message);
            Assert.IsType<InvalidOperationException>(exception.InnerException);
        }

        [Fact]
        public void FindFifthYearStudentsWheServed_ShouldFilterCorrectly()
        {
            var students = new List<Student>
            {
                new Student("Марія", "Сидорова", "P001", "S1", 5, "Моб001"),
                new Student("Андрій", "Демченко", "P002", "S2", 4, "Моб002"),
                new Student("Віталій", "Кличко", "P003", "S3", 5, "N/A"),
                new Student("Ольга", "Лисак", "P004", "S4", 5, "Моб004")
            };
            var mockContext = new Mock<IEntityContext<Student>>();
            mockContext.Setup(c => c.GetAll()).Returns(students);
            var service = new StudentService(mockContext.Object);

            var result = service.FindFifthYearStudentsWheServed();

            Assert.Equal(2, result.Count);
            Assert.Contains(result, s => s.LastName == "Сидорова");
            Assert.DoesNotContain(result, s => s.LastName == "Кличко");
        }

        [Fact]
        public void Find_WhenPersonNotFound_ShouldThrowBusinessLogicException()
        {
            var mockContext = new Mock<IEntityContext<Student>>();
            mockContext.Setup(c => c.GetAll()).Returns(new List<Student>());
            var service = new StudentService(mockContext.Object);

            var exception = Assert.Throws<BusinessLogicException>(
                () => service.Find("Неіснуючий", "Студент", "000"));

            Assert.Contains("Person not found.", exception.Message);
        }

        [Fact]
        public void RecitePoems_ShouldReturnCorrectPoem()
        {
            var student = new Student("Іван", "Франко", "PF123456", "ID001", 3, "N/A");
            var mockContext = new Mock<IEntityContext<Student>>();
            mockContext.Setup(c => c.GetAll()).Returns(new List<Student> { student });
            var service = new StudentService(mockContext.Object);

            var dto = service.Find("Іван", "Франко", "PF123456");

            var result = service.RecitePoems(dto);

            Assert.Contains("декламує", result);
        }

        [Fact]
        public void Remove_WhenPersonNotFound_ShouldThrowBusinessLogicException()
        {
            var mockContext = new Mock<IEntityContext<Student>>();
            mockContext.Setup(c => c.GetAll()).Returns(new List<Student>());
            var service = new StudentService(mockContext.Object);

            var exception = Assert.Throws<BusinessLogicException>(
                () => service.Remove("Неіснуючий", "Студент", "000"));

            Assert.Contains("Person not found for removal.", exception.Message);
            mockContext.Verify(c => c.SaveAll(It.IsAny<List<Student>>()), Times.Never);
        }

        [Fact]
        public void Remove_WhenPersonExists_ShouldCallContextSaveAllWithReducedCount()
        {
            var studentToRemove = new Student("Іван", "Франко", "PF123456", "ID001", 3, "N/A");
            var entities = new List<Student> { studentToRemove, new Student("Леся", "Українка", "PU654321", "ID002", 5, "Моб007") };
            var mockContext = new Mock<IEntityContext<Student>>();
            mockContext.Setup(c => c.GetAll()).Returns(entities);
            var service = new StudentService(mockContext.Object);

            service.Remove("Іван", "Франко", "PF123456");

            mockContext.Verify(c => c.SaveAll(It.Is<List<Student>>(l => l.Count == 1)), Times.Once);
        }
    }
}
