using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.IRepositorys;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

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
            string query = "SELECT username FROM User WHERE id = @userId";
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    var result = cmd.ExecuteScalar();
                    return result?.ToString();
                }
            }
        }

        public UserDTO AuthenticateUser(string username)
        {
            string query = "SELECT id, username, password, address FROM User WHERE username = @username";

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@username", username);

                    using (var reader = cmd.ExecuteReader())
                    {
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
                    }
                }
            }

            return null;
        }


        public void AddUser(UserDTO user)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            var query = "INSERT INTO user (username, password, address) VALUES (@username, @password, @address)";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@username", user.Username);
            cmd.Parameters.AddWithValue("@password", user.Password); // This should now be the hashed password
            cmd.Parameters.AddWithValue("@address", user.Address);

            cmd.ExecuteNonQuery();
        }


    }
}