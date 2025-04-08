using BusinessLogicLayer.DTOs;
using System.Collections.Generic;

namespace BusinessLogicLayer.IRepositorys
{
    public interface IUserRepository
    {
        string GetUsernameById(int userId);
        UserDTO AuthenticateUser(string username);
        void AddUser(UserDTO user);

    }
}