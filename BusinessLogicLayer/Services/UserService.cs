using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.IRepositorys;
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

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserDTO Login(string username, string password)
        {
            return _userRepository.AuthenticateUser(username, password);
        }

    }
}
