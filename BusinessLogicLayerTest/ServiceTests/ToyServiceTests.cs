using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services;
using BusinessLogicLayerTest.FakeToysRepo_s;
using BusinessLogicLayerTest.FakeToysRepos;

namespace BusinessLogicLayerTest.ServiceTests
{
    [TestClass]
    public class ToyServiceTests
    {
        [TestMethod]
        public void GetAllToys_WithToys_ReturnsToysWithUsernames()
        {
            // Arrange  
            var toyRepo = new FakeToyRepository_TwoToys();
            var userRepo = new FakeUserRepository_Static();
            var service = new ToyService(toyRepo, userRepo);

            // Act  
            var result = service.GetAllToys();

            // Assert  
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("User1", result[0].Username);
        }

        [TestMethod]
        public void GetAllToys_WithEmptyList_ReturnsEmpty()
        {
            // Arrange  
            var toyRepo = new FakeToyRepository_Empty();
            var userRepo = new FakeUserRepository_Static();
            var service = new ToyService(toyRepo, userRepo);

            // Act  
            var result = service.GetAllToys();

            // Assert  
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetToysByUserId_ExistingUser_ReturnsOnlyTheirToys()
        {
            // Arrange  
            var toyRepo = new FakeToyRepository_TwoToys();
            var userRepo = new FakeUserRepository_Static();
            var service = new ToyService(toyRepo, userRepo);

            // Act  
            var result = service.GetToysByUserId(1);

            // Assert  
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Lego", result[0].Name);
        }

        [TestMethod]
        public void GetToysByUserId_NonExistingUser_ReturnsEmpty()
        {
            // Arrange  
            var toyRepo = new FakeToyRepository_Empty();
            var userRepo = new FakeUserRepository_Static();
            var service = new ToyService(toyRepo, userRepo);

            // Act  
            var result = service.GetToysByUserId(99);

            // Assert  
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetToyById_ExistingToy_ReturnsToy()
        {
            // Arrange  
            var toyRepo = new FakeToyRepository_TwoToys();
            var userRepo = new FakeUserRepository_Static();
            var service = new ToyService(toyRepo, userRepo);

            // Act  
            var result = service.GetToyById(1);

            // Assert  
            Assert.IsNotNull(result);
            Assert.AreEqual("Lego", result.Name);
        }

        [TestMethod]
        public void GetToyById_NonExisting_ReturnsNull()
        {
            // Arrange  
            var toyRepo = new FakeToyRepository_Empty();
            var userRepo = new FakeUserRepository_Static();
            var service = new ToyService(toyRepo, userRepo);

            // Act  
            var result = service.GetToyById(123);

            // Assert  
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AddToy_WithValidToy_CallsAddWithCorrectData()
        {
            // Arrange  
            var toy = new ToyDTO { Id = 5, Name = "Puzzle", UserId = 3 };
            var toyRepo = new FakeToyRepository_RecordsAdd();
            var userRepo = new FakeUserRepository_Static();
            var service = new ToyService(toyRepo, userRepo);

            // Act  
            service.AddToy(toy);

            // Assert  
            Assert.IsNotNull(toyRepo.AddedToy);
            Assert.AreEqual("Puzzle", toyRepo.AddedToy.Name);
        }

        [TestMethod]
        public void UpdateToy_WithValidData_CallsUpdate()
        {
            // Arrange  
            var toy = new ToyDTO { Id = 1, Name = "UpdatedName", UserId = 1 };
            var toyRepo = new FakeToyRepository_RecordsUpdate();
            var userRepo = new FakeUserRepository_Static();
            var service = new ToyService(toyRepo, userRepo);

            // Act  
            service.UpdateToy(toy);

            // Assert  
            Assert.AreEqual("UpdatedName", toyRepo.UpdatedToy.Name);
        }
    }
}
