using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.IRepositorys;
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
            string query = "SELECT id, name, image, `condition`, user_id FROM Toy";

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
                                Image = reader.IsDBNull(reader.GetOrdinal("image")) ? null : reader.GetString("image"),
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

        //add toy
        public void AddToy(ToyDTO toy)
        {
            string query = "INSERT INTO Toy (name, image, `condition`, user_id) VALUES (@name, @image, @condition, @userId)";
            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@name", toy.Name);
                        cmd.Parameters.AddWithValue("@image", string.IsNullOrEmpty(toy.Image) ? DBNull.Value : toy.Image);
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


    }
}
