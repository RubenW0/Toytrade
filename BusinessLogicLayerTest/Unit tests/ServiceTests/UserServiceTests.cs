using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services;
using BusinessLogicLayerTest.FakeUserRepositorys;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging.Abstractions;


namespace BusinessLogicLayerTest.ServiceTests
{
    [TestClass]
    public class UserServiceTests
    {
        private class FakePasswordHasher : IPasswordHasher<UserDTO>
        {
            public string HashPassword(UserDTO user, string password)
            {
                return $"hashed-{password}";
            }

            public PasswordVerificationResult VerifyHashedPassword(UserDTO user, string hashedPassword, string providedPassword)
            {
                return hashedPassword == $"hashed-{providedPassword}"
                    ? PasswordVerificationResult.Success
                    : PasswordVerificationResult.Failed;
            }
        }

        [TestMethod]
        public void Login_WrongPassword_ReturnsNull()
        {
            // Arrange  
            var repo = new FakeUserRepository_OneUser();
            var hasher = new FakePasswordHasher();

            var user = repo.AuthenticateUser("SingleUser");
            user.Password = "hashed-correctpassword";

            var service = new UserService(repo, hasher);

            // Act  
            var result = service.Login("SingleUser", "wrongpassword");

            // Assert  
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Login_UserDoesNotExist_ReturnsNull()
        {
            // Arrange  
            var repo = new FakeUserRepository_Empty();
            var hasher = new FakePasswordHasher();
            var service = new UserService(repo, hasher);

            // Act  
            var result = service.Login("NonExistent", "password");

            // Assert  
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Register_UserIsHashedAndAdded()
        {
            // Arrange  
            var repo = new FakeUserRepository_TwoUsers();
            var hasher = new FakePasswordHasher();
            var service = new UserService(repo, hasher);

            var newUser = new UserDTO { Id = 3, Username = "User3", Password = "plaintext" };

            // Act  
            service.Register(newUser);

            // Assert  
            Assert.IsNotNull(repo.AddedUser);
            Assert.AreEqual("User3", repo.AddedUser.Username);
            Assert.AreEqual("hashed-plaintext", repo.AddedUser.Password);
        }

        [TestMethod]
        public void GetAllUsers_FromRepoWithTwoUsers_ReturnsTwoUsers()
        {
            // Arrange  
            var repo = new FakeUserRepository_TwoUsers();
            var hasher = new FakePasswordHasher();
            var service = new UserService(repo, hasher);

            // Act  
            List<UserDTO> users = service.GetAllUsers();

            // Assert  
            Assert.AreEqual(2, users.Count);
            Assert.AreEqual("User1", users[0].Username);
            Assert.AreEqual("User2", users[1].Username);
        }

        [TestMethod]
        public void GetAllUsers_FromEmptyRepo_ReturnsEmptyList()
        {
            // Arrange  
            var repo = new FakeUserRepository_Empty();
            var hasher = new FakePasswordHasher();
            var service = new UserService(repo, hasher);

            // Act  
            List<UserDTO> users = service.GetAllUsers();

            // Assert  
            Assert.IsNotNull(users);
            Assert.AreEqual(0, users.Count);
        }
    }
}
