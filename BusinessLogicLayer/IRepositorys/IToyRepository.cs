using BusinessLogicLayer.DTOs;
using System.Collections.Generic;

namespace BusinessLogicLayer.IRepositorys
{
    public interface IToyRepository
    {
        List<ToyDTO> GetAllToys();
    }
}