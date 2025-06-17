using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.IRepositories;
using System.Collections.Generic;

namespace BusinessLogicLayerTest.FakeUserRepositorys
{
    public class FakeUserRepository_OneUser : IUserRepository
    {
        public void AddUser(UserDTO user)
        {
        }

        public UserDTO AuthenticateUser(string username) =>
            new UserDTO { Id = 1, Username = "SingleUser", Password = "hashedpassword" };

        public List<UserDTO> GetAllUsers() => new List<UserDTO>
        {
            new UserDTO { Id = 1, Username = "SingleUser", Password = "hashedpassword" }
        };

        public string GetUsernameById(int userId) => userId == 1 ? "SingleUser" : null;

        public UserDTO GetUserById(int userId)
        {
            return null;
        }
    }
}
