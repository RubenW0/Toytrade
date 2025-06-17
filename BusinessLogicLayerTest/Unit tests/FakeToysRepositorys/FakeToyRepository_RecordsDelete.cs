using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayerTest.FakeToysRepos
{
    public class FakeToyRepository_RecordsDelete : IToyRepository
    {
        public int? DeletedToyId { get; private set; }

        public List<ToyDTO> GetAllToys() => new() {
        new ToyDTO { Id = 1, Name = "Lego", UserId = 1, ImagePath = "/images/lego.jpg" }
    };

        public void DeleteToy(int toyId) => DeletedToyId = toyId;

        public List<ToyDTO> GetToysByUserId(int userId) => throw new NotImplementedException();
        public ToyDTO GetToyById(int toyId) => throw new NotImplementedException();
        public void AddToy(ToyDTO toy) => throw new NotImplementedException();
        public void UpdateToy(ToyDTO toy) => throw new NotImplementedException();
    }
}