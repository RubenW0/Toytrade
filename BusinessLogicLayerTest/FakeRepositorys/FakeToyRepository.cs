using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.IRepositories;
using System.Collections.Generic;

public class FakeToyRepository : IToyRepository
{
    private List<ToyDTO> _toys = new List<ToyDTO>
    {
        new ToyDTO { Id = 1, Name = "Lego", UserId = 1 },
        new ToyDTO { Id = 2, Name = "Doll", UserId = 2 }
    };

    public List<ToyDTO> GetAllToys()
    {
        return _toys.ToList();
    }

    public List<ToyDTO> GetToysByUserId(int userId)
    {
        return _toys.Where(t => t.UserId == userId).ToList();
    }

    public void AddToy(ToyDTO toy)
    {
        _toys.Add(toy);
    }

    public void UpdateToy(ToyDTO toy)
    {
        var existing = _toys.FirstOrDefault(t => t.Id == toy.Id);
        if (existing != null)
        {
            existing.Name = toy.Name;
            existing.ImagePath = toy.ImagePath;
        }
    }

    public void DeleteToy(int toyId)
    {
        var toy = _toys.FirstOrDefault(t => t.Id == toyId);
        if (toy != null)
        {
            _toys.Remove(toy);
        }
    }
}