using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.IRepositories;
using System.Collections.Generic;

public class FakeToyRepository : IToyRepository
{
    private List<ToyDTO> _toys = new List<ToyDTO>();

    public List<ToyDTO> GetAllToys()
    {
        return _toys;
    }


    public List<ToyDTO> GetToysByUserId(int userId)
    {
        return new List<ToyDTO>(); 
    }

    public void AddToy(ToyDTO toy) 
    {
        _toys.Add(toy);
    }


    public void UpdateToy(ToyDTO toy) { }

    public void DeleteToy(int toyId) { }
}
