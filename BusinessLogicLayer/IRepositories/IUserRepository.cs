using BusinessLogicLayer.DTOs;
using System.Collections.Generic;

namespace BusinessLogicLayer.IRepositories
{
    public interface IUserRepository
    {
        string GetUsernameById(int userId);
        UserDTO AuthenticateUser(string username);
        void AddUser(UserDTO user);
        public List<UserDTO> GetAllUsers();
    }
}