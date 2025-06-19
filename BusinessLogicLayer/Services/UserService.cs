using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.IRepositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<UserDTO> _passwordHasher;

        public UserService(IUserRepository userRepository, IPasswordHasher<UserDTO> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public UserDTO Login(string username, string password)
        {
            var user = _userRepository.AuthenticateUser(username);

            if (user == null)
                return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);

            return result == PasswordVerificationResult.Success ? user : null;
        }

        public void Register(UserDTO user)
        {
            user.Password = _passwordHasher.HashPassword(user, user.Password);
            _userRepository.AddUser(user);
        }

        public List<UserDTO> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public UserDTO GetUserById(int userId)
        {
            return _userRepository.GetUserById(userId);
        }
    }
}
