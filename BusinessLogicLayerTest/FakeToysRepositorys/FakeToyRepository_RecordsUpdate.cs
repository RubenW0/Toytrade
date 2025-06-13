using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayerTest.FakeToysRepos
{
    public class FakeToyRepository_RecordsUpdate : IToyRepository
    {
        public ToyDTO UpdatedToy { get; private set; }

        public void UpdateToy(ToyDTO toy) => UpdatedToy = toy;

        public List<ToyDTO> GetAllToys() => throw new NotImplementedException();
        public List<ToyDTO> GetToysByUserId(int userId) => throw new NotImplementedException();
        public ToyDTO GetToyById(int toyId) => throw new NotImplementedException();
        public void AddToy(ToyDTO toy) => throw new NotImplementedException();
        public void DeleteToy(int toyId) => throw new NotImplementedException();
    }
}