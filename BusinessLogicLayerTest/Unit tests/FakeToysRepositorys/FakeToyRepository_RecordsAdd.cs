using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayerTest.FakeToysRepos
{
    public class FakeToyRepository_RecordsAdd : IToyRepository
    {
        public ToyDTO AddedToy { get; private set; }

        public void AddToy(ToyDTO toy) => AddedToy = toy;

        public List<ToyDTO> GetAllToys() => throw new NotImplementedException();
        public List<ToyDTO> GetToysByUserId(int userId) => throw new NotImplementedException();
        public ToyDTO GetToyById(int toyId) => throw new NotImplementedException();
        public void UpdateToy(ToyDTO toy) => throw new NotImplementedException();
        public void DeleteToy(int toyId) => throw new NotImplementedException();
    }
}