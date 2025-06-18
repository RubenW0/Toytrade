using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.IRepositories;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace DataAccessLayer.Repositorys
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(IConfiguration configuration, ILogger<UserRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("MySqlConnection");
            _logger = logger;
        }

        private void LogErrorWithMethodName(Exception ex, string? extraMessage = null, [CallerMemberName] string callerName = "")
        {
            var msg = $"Exception in {callerName}";
            if (!string.IsNullOrEmpty(extraMessage))
                msg += $": {extraMessage}";

            _logger.LogError(ex, msg);
        }

        public string GetUsernameById(int userId)
        {
            try
            {
                string query = "SELECT username FROM User WHERE id = @userId";
                using var connection = new MySqlConnection(_connectionString);
                connection.Open();
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@userId", userId);
                var result = cmd.ExecuteScalar();
                return result?.ToString();
            }
            catch (Exception ex)
            {
                LogErrorWithMethodName(ex, $"Error while retrieving username for user with ID {userId}.");
                throw;
            }
        }

        public UserDTO AuthenticateUser(string username)
        {
            try
            {
                string query = "SELECT id, username, password, address FROM User WHERE username = @username";

                using var connection = new MySqlConnection(_connectionString);
                connection.Open();
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@username", username);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new UserDTO
                    {
                        Id = reader.GetInt32("id"),
                        Username = reader.GetString("username"),
                        Password = reader.GetString("password"),
                        Address = reader.GetString("address")
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                LogErrorWithMethodName(ex, $"Error while authenticating user with username {username}.");
                throw;
            }
        }

        public void AddUser(UserDTO user)
        {
            try
            {
                string query = "INSERT INTO user (username, password, address) VALUES (@username, @password, @address)";
                using var connection = new MySqlConnection(_connectionString);
                connection.Open();
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@username", user.Username);
                cmd.Parameters.AddWithValue("@password", user.Password);
                cmd.Parameters.AddWithValue("@address", user.Address);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LogErrorWithMethodName(ex, "Error while adding user.");
                throw;
            }
        }

        public List<UserDTO> GetAllUsers()
        {
            try
            {
                var users = new List<UserDTO>();
                string query = "SELECT id, username, password, address FROM User";
                using var connection = new MySqlConnection(_connectionString);
                connection.Open();
                using var cmd = new MySqlCommand(query, connection);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    users.Add(new UserDTO
                    {
                        Id = reader.GetInt32("id"),
                        Username = reader.GetString("username"),
                        Password = reader.GetString("password"),
                        Address = reader.GetString("address")
                    });
                }
                return users;
            }
            catch (Exception ex)
            {
                LogErrorWithMethodName(ex, "Error while retrieving all users.");
                throw;
            }
        }

        public UserDTO GetUserById(int userId)
        {
            try
            {
                string query = "SELECT id, username, password, address FROM User WHERE id = @userId";
                using var connection = new MySqlConnection(_connectionString);
                connection.Open();
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@userId", userId);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new UserDTO
                    {
                        Id = reader.GetInt32("id"),
                        Username = reader.GetString("username"),
                        Password = reader.GetString("password"),
                        Address = reader.GetString("address")
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                LogErrorWithMethodName(ex, $"Error while retrieving user with ID {userId}.");
                throw;
            }
        }
    }
}