using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.IRepositories;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;


public class TradeRequestRepository : ITradeRequestRepository
{
    private readonly string _connectionString;

    public TradeRequestRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("MySqlConnection");
    }

    public List<ToyDTO> GetOfferedToysByTradeRequestId(int tradeRequestId)
    {
        var toys = new List<ToyDTO>();
        string query = @"SELECT t.id, t.name, t.image_path
                     FROM toy t
                     JOIN traderequest_offeredtoy ot ON t.id = ot.toy_id
                     WHERE ot.traderequest_id = @tradeRequestId";

        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@tradeRequestId", tradeRequestId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        toys.Add(new ToyDTO
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name"),
                            ImagePath = reader.GetString("image_path")
                        });
                    }
                }
            }
        }

        return toys;
    }

    public List<ToyDTO> GetRequestedToysByTradeRequestId(int tradeRequestId)
    {
        var toys = new List<ToyDTO>();
        string query = @"SELECT t.id, t.name, t.image_path
                     FROM toy t
                     JOIN traderequest_requestedtoy rt ON t.id = rt.toy_id
                     WHERE rt.traderequest_id = @tradeRequestId";

        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@tradeRequestId", tradeRequestId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        toys.Add(new ToyDTO
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name"),
                            ImagePath = reader.GetString("image_path")
                        });
                    }
                }
            }
        }

        return toys;
    }

    public List<TradeRequestDTO> GetTradeRequestsByUserId(int userId)
    {
        var requests = new List<TradeRequestDTO>();
        string query = @"SELECT id, status, user_id_requester, user_id_receiver
                     FROM traderequest
                     WHERE user_id_requester = @userId OR user_id_receiver = @userId";

        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@userId", userId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        requests.Add(new TradeRequestDTO
                        {
                            Id = reader.GetInt32("id"),
                            Status = reader.GetString("status"),
                            RequesterId = reader.GetInt32("user_id_requester"),
                            ReceiverId = reader.GetInt32("user_id_receiver")
                        });
                    }
                }
            }
        }

        return requests;
    }

}
