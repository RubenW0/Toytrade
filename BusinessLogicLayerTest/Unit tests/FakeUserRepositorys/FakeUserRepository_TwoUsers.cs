using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.IRepositories;
using System.Collections.Generic;

namespace BusinessLogicLayerTest.FakeUserRepositorys
{
    public class FakeUserRepository_TwoUsers : IUserRepository
    {
        public UserDTO AddedUser { get; private set; }

        public void AddUser(UserDTO user) => AddedUser = user;

        public UserDTO AuthenticateUser(string username)
        {
            if (username == "User1")
                return new UserDTO { Id = 1, Username = "User1", Password = "hashedpassword1" };
            if (username == "User2")
                return new UserDTO { Id = 2, Username = "User2", Password = "hashedpassword2" };
            return null;
        }

        public List<UserDTO> GetAllUsers() => new List<UserDTO>
        {
            new UserDTO { Id = 1, Username = "User1", Password = "hashedpassword1" },
            new UserDTO { Id = 2, Username = "User2", Password = "hashedpassword2" }
        };

        public string GetUsernameById(int userId)
        {
            if (userId == 1) return "User1";
            if (userId == 2) return "User2";
            return null;
        }

        public UserDTO GetUserById(int userId)
        {
            return null;
        }
    }
}
