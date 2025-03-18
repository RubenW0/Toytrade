using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.IRepositorys; 
using System.Collections.Generic;

namespace BusinessLogicLayer.Services
{
    public class ToyService
    {
        private readonly IToyRepository _toyRepository;

        public ToyService(IToyRepository toyRepository) 
        {
            _toyRepository = toyRepository;
        }

        public List<ToyDTO> GetAllToys()
        {
            return _toyRepository.GetAllToys();
        }
    }
}
