using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.IRepositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<UserDTO> _passwordHasher;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, IPasswordHasher<UserDTO> passwordHasher, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        private void LogErrorWithMethodName(Exception ex, string? extraMessage = null, [System.Runtime.CompilerServices.CallerMemberName] string callerName = "")
        {
            var msg = $"Exception in {callerName}";
            if (!string.IsNullOrEmpty(extraMessage))
                msg += $": {extraMessage}";
            _logger.LogError(ex, msg);
        }

        public UserDTO Login(string username, string password)
        {
            try
            {
                var user = _userRepository.AuthenticateUser(username);

                if (user == null)
                    return null;

                var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);

                return result == PasswordVerificationResult.Success ? user : null;
            }
            catch (Exception ex)
            {
                LogErrorWithMethodName(ex, $"Error while logging in user '{username}' in service.");
                throw;
            }
        }

        public void Register(UserDTO user)
        {
            try
            {
                user.Password = _passwordHasher.HashPassword(user, user.Password);
                _userRepository.AddUser(user);
            }
            catch (Exception ex)
            {
                LogErrorWithMethodName(ex, $"Error while registering user '{user.Username}' in service.");
                throw;
            }
        }

        public List<UserDTO> GetAllUsers()
        {
            try
            {
                return _userRepository.GetAllUsers();
            }
            catch (Exception ex)
            {
                LogErrorWithMethodName(ex, "Error while retrieving all users in service.");
                throw;
            }
        }

        public UserDTO GetUserById(int userId)
        {
            try
            {
                return _userRepository.GetUserById(userId);
            }
            catch (Exception ex)
            {
                LogErrorWithMethodName(ex, $"Error while retrieving user with ID {userId} in service.");
                throw;
            }
        }
    }
}
