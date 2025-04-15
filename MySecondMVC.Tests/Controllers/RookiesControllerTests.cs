using Moq;
using Microsoft.AspNetCore.Mvc;
using MySecondMVC.Controllers;
using MySecondMVC.Models;
using MySecondMVC.Services;
using MySecondMVC.Enums;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Http;

namespace MySecondMVC.Tests.Controllers
{
    public class RookiesControllerTests
    {
        private Mock<IPersonService> _mockService;
        private RookiesController _controller;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IPersonService>();
            _controller = new RookiesController(_mockService.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _controller?.Dispose();
        }

        [Test]
        public void Index_ReturnsViewWithPagedPeople()
        {
            // Arange
            var dummyPeople = new List<Person> { new Person { FirstName = "Minh", LastName = "Nguyen", DateOfBirth = new DateOnly(2000, 1, 1), Gender = Gender.Male, PhoneNumber = "123", BirthPlace = "Hanoi", IsGraduated = true } };
            _mockService.Setup(s => s.GetPaged(1, 4)).Returns(dummyPeople);
            _mockService.Setup(s => s.GetTotalCount()).Returns(1);

            // Act
            var result = _controller.Index(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Model, Is.EqualTo(dummyPeople));
        }

        [Test]
        public void PersonDetails_ValidId_ReturnsViewWithPerson()
        {
            var person = GetDummyPerson();
            _mockService.Setup(s => s.GetById(person.Id)).Returns(person);

            var result = _controller.PersonDetails(person.Id);

            Assert.IsInstanceOf<ViewResult>(result);
            Assert.That(((ViewResult)result).Model, Is.EqualTo(person));
        }

        [Test]
        public void PersonDetails_IdNotFound_ReturnsNotFound()
        {
            _mockService.Setup(s => s.GetById(It.IsAny<Guid>())).Returns((Person)null);

            var result = _controller.PersonDetails(Guid.NewGuid());

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void CreatePerson_Post_InvalidModel_ReturnsView()
        {
            _controller.ModelState.AddModelError("FirstName", "Required");
            var person = GetDummyPerson();

            var result = _controller.CreatePerson(person);

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void CreatePerson_Post_ValidModel_RedirectsToIndex()
        {
            var person = GetDummyPerson();

            var result = _controller.CreatePerson(person);

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirect = (RedirectToActionResult)result;
            Assert.That(redirect.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public void EditPerson_Get_ValidId_ReturnsViewWithPerson()
        {
            var person = GetDummyPerson();
            _mockService.Setup(s => s.GetById(person.Id)).Returns(person);

            var result = _controller.EditPerson(person.Id);

            Assert.IsInstanceOf<ViewResult>(result);
            Assert.That(((ViewResult)result).Model, Is.EqualTo(person));
        }

        [Test]
        public void EditPerson_Get_InvalidId_ReturnsNotFound()
        {
            _mockService.Setup(s => s.GetById(It.IsAny<Guid>())).Returns((Person)null);

            var result = _controller.EditPerson(Guid.NewGuid());

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void EditPerson_Post_InvalidModel_ReturnsViewWithPerson()
        {
            _controller.ModelState.AddModelError("FirstName", "Required");

            var person = GetDummyPerson();
            var result = _controller.EditPerson(person);

            Assert.IsInstanceOf<ViewResult>(result);
            Assert.That(((ViewResult)result).Model, Is.EqualTo(person));
        }

        [Test]
        public void EditPerson_Post_ValidModel_RedirectsToIndex()
        {
            var person = GetDummyPerson();

            var result = _controller.EditPerson(person);

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.That(((RedirectToActionResult)result).ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public void DeletePerson_ValidId_ReturnsViewWithPerson()
        {
            var person = GetDummyPerson();
            _mockService.Setup(s => s.GetById(person.Id)).Returns(person);

            var result = _controller.DeletePerson(person.Id);

            Assert.IsInstanceOf<ViewResult>(result);
            Assert.That(((ViewResult)result).Model, Is.EqualTo(person));
        }

        [Test]
        public void DeletePerson_InvalidId_ReturnsNotFound()
        {
            _mockService.Setup(s => s.GetById(It.IsAny<Guid>())).Returns((Person)null);

            var result = _controller.DeletePerson(Guid.NewGuid());

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void DeleteConfirmed_ValidId_RedirectsToConfirmation()
        {
            var person = GetDummyPerson();
            _mockService.Setup(s => s.GetById(person.Id)).Returns(person);

            _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

            var result = _controller.DeleteConfirmed(person.Id);

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.That(((RedirectToActionResult)result).ActionName, Is.EqualTo("DeleteConfirmation"));
        }

        [Test]
        public void DeleteConfirmed_InvalidId_ReturnsNotFound()
        {
            _mockService.Setup(s => s.GetById(It.IsAny<Guid>())).Returns((Person)null);

            var result = _controller.DeleteConfirmed(Guid.NewGuid());

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void GetMales_ReturnsFilteredListView()
        {
            var males = new List<Person> { GetDummyPerson() };
            _mockService.Setup(s => s.GetMales()).Returns(males);

            var result = _controller.GetMales() as ViewResult;

            Assert.IsNotNull(result);
            Assert.That(result.ViewName, Is.EqualTo("FilteredList"));
            Assert.That(result.Model, Is.EqualTo(males));
        }

        [Test]
        public void GetOldest_ReturnsPersonDetailsView()
        {
            var person = GetDummyPerson();
            _mockService.Setup(s => s.GetOldest()).Returns(person);

            var result = _controller.GetOldest() as ViewResult;

            Assert.IsNotNull(result);
            Assert.That(result.ViewName, Is.EqualTo("PersonDetails"));
            Assert.That(result.Model, Is.EqualTo(person));
        }

        [Test]
        public void GetFullNames_ReturnsFullNamesViewWithNames()
        {
            var fullNames = new List<string> { "Nguyen Minh", "Le Van" };
            _mockService.Setup(s => s.GetFullNames()).Returns(fullNames);

            var result = _controller.GetFullNames() as ViewResult;

            Assert.IsNotNull(result);
            Assert.That(result!.ViewName, Is.EqualTo("FullNames"));
            Assert.That(result.Model, Is.EqualTo(fullNames));
        }


        private Person GetDummyPerson() => new()
        {
            FirstName = "Minh",
            LastName = "Nguyen",
            Gender = Gender.Male,
            DateOfBirth = new DateOnly(2000, 1, 1),
            PhoneNumber = "0123456789",
            BirthPlace = "Vietnam",
            IsGraduated = true
        };
    }
}