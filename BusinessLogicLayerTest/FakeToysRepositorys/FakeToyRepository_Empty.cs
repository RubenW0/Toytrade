using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayerTest.FakeToysRepo_s
{
    public class FakeToyRepository_Empty : IToyRepository
    {
        public List<ToyDTO> GetAllToys() => new();
        public List<ToyDTO> GetToysByUserId(int userId) => new();
        public ToyDTO GetToyById(int toyId) => null;
        public void AddToy(ToyDTO toy) => throw new NotImplementedException();
        public void UpdateToy(ToyDTO toy) => throw new NotImplementedException();
        public void DeleteToy(int toyId) => throw new NotImplementedException();
    }
}
