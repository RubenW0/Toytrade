using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.IRepositories;

public class FakeUserRepository_Static : IUserRepository
{
    private readonly Dictionary<int, string> _users = new()
    {
        { 1, "User1" },
        { 2, "User2" },
        { 3, "User3" }
    };

    public string GetUsernameById(int userId)
    {
        return _users.TryGetValue(userId, out var username) ? username : null;
    }

    public UserDTO AuthenticateUser(string username)
    {
        return null;
    }

    public void AddUser(UserDTO user)
    {
    }

    public List<UserDTO> GetAllUsers()
    {
        return new List<UserDTO>();
    }

    public UserDTO GetUserById(int userId)
    {
        return null;
    }
}
