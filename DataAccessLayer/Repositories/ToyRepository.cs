using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.IRepositories;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace DataAccessLayer.Repositorys
{
    public class ToyRepository : IToyRepository 
    {
        private readonly string _connectionString;

        public ToyRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MySqlConnection");
        }

        public List<ToyDTO> GetAllToys()
        {
            var toys = new List<ToyDTO>();
            string query = "SELECT id, name, image_path AS image, `condition`, user_id FROM Toy";

            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (var cmd = new MySqlCommand(query, connection))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            toys.Add(new ToyDTO
                            {
                                Id = reader.GetInt32("id"),
                                Name = reader.GetString("name"),
                                ImagePath = reader.IsDBNull(reader.GetOrdinal("image")) ? null : reader.GetString("image"),
                                Condition = reader.GetString("condition"),
                                UserId = reader.GetInt32("user_id")
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Fout bij ophalen van speelgoed", ex);
                }
            }

            return toys;
        }

        public List<ToyDTO> GetToysByUserId(int userId)
        {
            var toys = new List<ToyDTO>();
            string query = "SELECT id, name, image_path AS image, `condition`, user_id FROM Toy WHERE user_id = @userId";

            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                toys.Add(new ToyDTO
                                {
                                    Id = reader.GetInt32("id"),
                                    Name = reader.GetString("name"),
                                    ImagePath = reader.IsDBNull(reader.GetOrdinal("image")) ? null : reader.GetString("image"),
                                    Condition = reader.GetString("condition"),
                                    UserId = reader.GetInt32("user_id")
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Fout bij ophalen van speelgoed per gebruiker", ex);
                }
            }

            return toys;
        }

        public ToyDTO GetToyById(int toyId)
        {
            string query = "SELECT id, name, image_path, `condition`, user_id FROM toy WHERE id = @toyId";

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@toyId", toyId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new ToyDTO
                            {
                                Id = reader.GetInt32("id"),
                                Name = reader.GetString("name"),
                                ImagePath = reader.GetString("image_path"),
                                Condition = reader.GetString("condition"),
                                UserId = reader.GetInt32("user_id")
                            };
                        }
                    }
                }
            }

            return null; 
        }

        public void AddToy(ToyDTO toy)
        {
            string query = "INSERT INTO Toy (name, image_path, `condition`, user_id) VALUES (@name, @image, @condition, @userId)";
            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@name", toy.Name);
                        cmd.Parameters.AddWithValue("@image", string.IsNullOrEmpty(toy.ImagePath) ? DBNull.Value : toy.ImagePath);
                        cmd.Parameters.AddWithValue("@condition", toy.Condition);
                        cmd.Parameters.AddWithValue("@userId", toy.UserId);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Fout bij toevoegen van speelgoed", ex);
                }
            }
        }

        public void UpdateToy(ToyDTO toy)
        {
            string query;

            if (!string.IsNullOrEmpty(toy.ImagePath))
            {
                query = "UPDATE Toy SET name = @name, image_path = @image, `condition` = @condition WHERE id = @id";
            }
            else
            {
                query = "UPDATE Toy SET name = @name, `condition` = @condition WHERE id = @id";
            }

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@id", toy.Id);
                    cmd.Parameters.AddWithValue("@name", toy.Name);
                    cmd.Parameters.AddWithValue("@condition", toy.Condition);

                    if (!string.IsNullOrEmpty(toy.ImagePath))
                    {
                        cmd.Parameters.AddWithValue("@image", toy.ImagePath);
                    }

                    cmd.ExecuteNonQuery();
                }
            }
        }


        public void DeleteToy(int toyId)
        {
            string query = "DELETE FROM Toy WHERE id = @id";

            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", toyId);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Fout bij verwijderen van speelgoed", ex);
                }
            }
        }


    }
}
