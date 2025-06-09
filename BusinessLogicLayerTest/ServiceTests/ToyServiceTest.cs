using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace BusinessLogicLayerTest.ServiceTests
{
    [TestClass]
    public class ToyServiceTest
    {
        private ToyService _service;
        private FakeToyRepository _toyRepository;
        private FakeUserRepository _userRepository;

        [TestInitialize]
        public void Setup()
        {
            _toyRepository = new FakeToyRepository();
            _userRepository = new FakeUserRepository();
            _service = new ToyService(_toyRepository, _userRepository, null);
        }

        [TestMethod]
        public void GetAllToys_ShouldReturnAllToys()
        {
            // Arrange
            _toyRepository = new FakeToyRepository();
            _userRepository = new FakeUserRepository();
            _service = new ToyService(_toyRepository, _userRepository, null);

            // Act
            var result = _service.GetAllToys();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Lego", result[0].Name);
            Assert.AreEqual("Doll", result[1].Name);
        }

        [TestMethod]
        public void GetToysByUserId_ShouldReturnOnlyToysOfThatUser()
        {
            // Arrange
            _toyRepository = new FakeToyRepository();
            _userRepository = new FakeUserRepository();
            _service = new ToyService(_toyRepository, _userRepository, null);

            // Act
            var result = _service.GetToysByUserId(1);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Lego", result[0].Name);
            Assert.AreEqual("User1", result[0].Username);
        }

        [TestMethod]
        public void AddToy_ShouldIncreaseToyCount()
        {
            // Arrange
            _toyRepository = new FakeToyRepository();
            _userRepository = new FakeUserRepository();
            _service = new ToyService(_toyRepository, _userRepository, null);
            var newToy = new ToyDTO { Id = 3, Name = "Puzzle", UserId = 3 };

            // Act
            _service.AddToy(newToy);
            var result = _service.GetAllToys();

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.IsTrue(result.Any(t => t.Name == "Puzzle"));
        }

        [TestMethod]
        public void UpdateToy_ShouldModifyToyData()
        {
            // Arrange
            _toyRepository = new FakeToyRepository();
            _userRepository = new FakeUserRepository();
            _service = new ToyService(_toyRepository, _userRepository, null);
            var updatedToy = new ToyDTO { Id = 1, Name = "Updated Lego", UserId = 1 };

            // Act
            _service.UpdateToy(updatedToy);
            var result = _service.GetAllToys().FirstOrDefault(t => t.Id == 1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Updated Lego", result.Name);
        }

        [TestMethod]
        public void DeleteToy_ShouldRemoveToy()
        {
            // Arrange
            _toyRepository = new FakeToyRepository();
            _userRepository = new FakeUserRepository();
            _service = new ToyService(_toyRepository, _userRepository, null);

            // Act
            _service.DeleteToy(1);
            var result = _service.GetAllToys();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsFalse(result.Any(t => t.Id == 1));
        }
    }
}