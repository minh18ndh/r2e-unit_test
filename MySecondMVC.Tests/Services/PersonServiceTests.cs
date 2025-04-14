using Moq;
using MySecondMVC.Models;
using MySecondMVC.Repositories;
using MySecondMVC.Services;
using MySecondMVC.Enums;

namespace MySecondMVC.Tests.Services
{
    public class PersonServiceTests
    {
        private Mock<IPersonRepository> _mockRepo;
        private PersonService _service;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IPersonRepository>();
            _service = new PersonService(_mockRepo.Object);
        }

        [Test]
        public void GetMales_ReturnsOnlyMales()
        {
            // Arrange
            var data = new List<Person>
            {
                new() { Gender = Gender.Male, FirstName = "A", LastName = "B", DateOfBirth = new DateOnly(2000,1,1), PhoneNumber = "", BirthPlace = "", IsGraduated = true },
                new() { Gender = Gender.Female, FirstName = "C", LastName = "D", DateOfBirth = new DateOnly(2001,1,1), PhoneNumber = "", BirthPlace = "", IsGraduated = true }
            };
            _mockRepo.Setup(r => r.GetAll()).Returns(data);

            // Act
            var result = _service.GetMales();

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.IsTrue(result.All(p => p.Gender == Gender.Male));
        }

        [Test]
        public void GetOldest_ReturnsPersonWithHighestAge()
        {
            var data = new List<Person>
            {
                new() { DateOfBirth = new DateOnly(1980, 1, 1), FirstName = "Old", LastName = "Man", Gender = Gender.Male, PhoneNumber = "", BirthPlace = "", IsGraduated = false },
                new() { DateOfBirth = new DateOnly(1990, 1, 1), FirstName = "Young", LastName = "Girl", Gender = Gender.Female, PhoneNumber = "", BirthPlace = "", IsGraduated = true }
            };
            _mockRepo.Setup(r => r.GetAll()).Returns(data);

            var result = _service.GetOldest();

            Assert.IsNotNull(result);
            Assert.That(result.FirstName, Is.EqualTo("Old"));
        }

        [Test]
        public void GetFullNames_ReturnsCorrectConcatenation()
        {
            var data = new List<Person>
            {
                new() { FirstName = "Minh", LastName = "Nguyen", DateOfBirth = new DateOnly(2000,1,1), Gender = Gender.Male, PhoneNumber = "", BirthPlace = "", IsGraduated = true }
            };
            _mockRepo.Setup(r => r.GetAll()).Returns(data);

            var result = _service.GetFullNames();

            Assert.That(result.First(), Is.EqualTo("Nguyen Minh"));
        }

        [Test]
        public void FilterByBirthYear_ReturnsCorrectList_WhenFilterIsBefore()
        {
            var data = new List<Person>
            {
                new() { DateOfBirth = new DateOnly(1999, 1, 1), FirstName = "Old", LastName = "Guy", Gender = Gender.Male, PhoneNumber = "", BirthPlace = "", IsGraduated = true },
                new() { DateOfBirth = new DateOnly(2005, 1, 1), FirstName = "New", LastName = "Kid", Gender = Gender.Male, PhoneNumber = "", BirthPlace = "", IsGraduated = false }
            };
            _mockRepo.Setup(r => r.GetAll()).Returns(data);

            var result = _service.FilterByBirthYear(2000, "before");

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].FirstName, Is.EqualTo("Old"));
        }

        [Test]
        public void GetById_ValidId_ReturnsCorrectPerson()
        {
            var id = Guid.NewGuid();
            var person = new Person
            {
                Id = id,
                FirstName = "Minh",
                LastName = "Nguyen",
                DateOfBirth = new DateOnly(2000, 1, 1),
                Gender = Gender.Male,
                PhoneNumber = "0123456789",
                BirthPlace = "Vietnam",
                IsGraduated = true
            };
            _mockRepo.Setup(r => r.GetById(id)).Returns(person);

            var result = _service.GetById(id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.FirstName, Is.EqualTo("Minh"));
        }

        [Test]
        public void Add_CallsRepositoryAdd_Once()
        {
            var person = new Person
            {
                FirstName = "Minh",
                LastName = "Nguyen",
                DateOfBirth = new DateOnly(2000, 1, 1),
                Gender = Gender.Male,
                PhoneNumber = "0123456789",
                BirthPlace = "Vietnam",
                IsGraduated = true
            };

            _service.Add(person);

            _mockRepo.Verify(r => r.Add(person), Times.Once);
        }

        [Test]
        public void Update_CallsRepositoryUpdate_Once()
        {
            var person = new Person
            {
                FirstName = "Minh",
                LastName = "Nguyen",
                DateOfBirth = new DateOnly(2000, 1, 1),
                Gender = Gender.Male,
                PhoneNumber = "0123456789",
                BirthPlace = "Vietnam",
                IsGraduated = true
            };

            _service.Update(person);

            _mockRepo.Verify(r => r.Update(person), Times.Once);
        }

        [Test]
        public void GetPaged_ReturnsCorrectSubsetOfPeople()
        {
            var data = new List<Person>
            {
                new() { FirstName = "Person1", LastName = "One", DateOfBirth = new DateOnly(2000,1,1), Gender = Gender.Male, PhoneNumber = "", BirthPlace = "", IsGraduated = true },
                new() { FirstName = "Person2", LastName = "Two", DateOfBirth = new DateOnly(2001,1,1), Gender = Gender.Female, PhoneNumber = "", BirthPlace = "", IsGraduated = true },
                new() { FirstName = "Person3", LastName = "Three", DateOfBirth = new DateOnly(2002,1,1), Gender = Gender.Male, PhoneNumber = "", BirthPlace = "", IsGraduated = false },
                new() { FirstName = "Person4", LastName = "Four", DateOfBirth = new DateOnly(2003,1,1), Gender = Gender.Female, PhoneNumber = "", BirthPlace = "", IsGraduated = false }
            };
            _mockRepo.Setup(r => r.GetAll()).Returns(data);

            var result = _service.GetPaged(2, 2); // page 2, pageSize 2 -> expect Person3 & Person4

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].FirstName, Is.EqualTo("Person3"));
            Assert.That(result[1].FirstName, Is.EqualTo("Person4"));
        }
    }
}