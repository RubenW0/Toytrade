using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.IRepositories;
using System.Collections.Generic;

public class FakeToyRepository_TwoToys : IToyRepository
{
    public List<ToyDTO> GetAllToys() => new()
    {
        new ToyDTO { Id = 1, Name = "Lego", UserId = 1 },
        new ToyDTO { Id = 2, Name = "Doll", UserId = 2 }
    };

    public List<ToyDTO> GetToysByUserId(int userId) => GetAllToys().Where(t => t.UserId == userId).ToList();
    public ToyDTO GetToyById(int toyId) => GetAllToys().FirstOrDefault(t => t.Id == toyId);
    public void AddToy(ToyDTO toy) => throw new NotImplementedException();
    public void UpdateToy(ToyDTO toy) => throw new NotImplementedException();
    public void DeleteToy(int toyId) => throw new NotImplementedException();
}
