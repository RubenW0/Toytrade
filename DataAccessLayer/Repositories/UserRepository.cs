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

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MySqlConnection");
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
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
                throw;
            }
        }
    }
}