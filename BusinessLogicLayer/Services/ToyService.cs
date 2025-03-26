using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.IRepositorys;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Services
{
    public class ToyService
    {
        private readonly IToyRepository _toyRepository;
        private readonly IUserRepository _userRepository;

        public ToyService(IToyRepository toyRepository, IUserRepository userRepository)
        {
            _toyRepository = toyRepository;
            _userRepository = userRepository;
        }

        public List<ToyDTO> GetAllToys()
        {
            var toys = _toyRepository.GetAllToys();

            foreach (var toy in toys)
            {
                toy.Username = _userRepository.GetUsernameById(toy.UserId);
            }

            return toys;
        }

        public void AddToy(ToyDTO toy)
        {
            _toyRepository.AddToy(toy);
        }

    }
}
