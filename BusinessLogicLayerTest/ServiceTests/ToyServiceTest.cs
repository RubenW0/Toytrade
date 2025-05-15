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
            _toyRepository.AddToy(new ToyDTO { Id = 1, Name = "Lego", UserId = 1 });
            _toyRepository.AddToy(new ToyDTO { Id = 2, Name = "Doll", UserId = 2 });

            // Act  
            var result = _service.GetAllToys();

            // Assert  
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("User1", result[0].Username);
            Assert.AreEqual("User2", result[1].Username);
        }


    }
}
