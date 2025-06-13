using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.IRepositories;
using System.Collections.Generic;

namespace BusinessLogicLayerTest.FakeUserRepositorys
{
    public class FakeUserRepository_Empty : IUserRepository
    {
        public void AddUser(UserDTO user)
        {
        }

        public UserDTO AuthenticateUser(string username) => null;

        public List<UserDTO> GetAllUsers() => new List<UserDTO>();

        public string GetUsernameById(int userId) => null;
    }
}
