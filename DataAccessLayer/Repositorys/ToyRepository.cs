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
            string query = "SELECT id, name, image, 'condition', owner_id FROM Toy";

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
                                //OwnerId = reader.GetInt32("owner_id")
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
    }
}
