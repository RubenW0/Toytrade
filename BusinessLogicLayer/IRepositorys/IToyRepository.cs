using BusinessLogicLayer.DTOs;
using System.Collections.Generic;

namespace BusinessLogicLayer.IRepositorys
{
    public interface IToyRepository
    {
        List<ToyDTO> GetAllToys();
        void AddToy(ToyDTO toy);
        void UpdateToy(ToyDTO toy);
        void DeleteToy(int toyId);
    }
}