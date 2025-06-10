using BusinessLogicLayer.DTOs;
using System.Collections.Generic;

namespace BusinessLogicLayer.IRepositories
{
    public interface IToyRepository
    {
        List<ToyDTO> GetAllToys();
        List<ToyDTO> GetToysByUserId(int userId);
        ToyDTO GetToyById(int toyId);
        void AddToy(ToyDTO toy);
        void UpdateToy(ToyDTO toy);
        void DeleteToy(int toyId);

    }
}