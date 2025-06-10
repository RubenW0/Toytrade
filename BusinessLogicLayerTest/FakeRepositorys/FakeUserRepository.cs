using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.IRepositories;

public class FakeUserRepository : IUserRepository
{
    public string GetUsernameById(int userId)
    {
        return $"User{userId}";
    }

    public UserDTO AuthenticateUser(string username)
    {
        // user authentication  
        return new UserDTO
        {
            Id = 1,
            Username = username,
            Password = "hashedpassword",
            Address = "123 Test Street"
        };
    }

    public void AddUser(UserDTO user)
    {
    }

    public List<UserDTO> GetAllUsers()
    {
        return new List<UserDTO>();
    }
}

