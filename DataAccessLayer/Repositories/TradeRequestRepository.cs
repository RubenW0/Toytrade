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

    public int CreateTradeRequest(int requesterId, int receiverId, List<int> offeredToyIds, List<int> requestedToyIds)
    {
        int tradeRequestId = 0;
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            // Begin een transactie zodat we alle stappen kunnen terugdraaien bij een fout
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    // 1. Voeg het basis ruilverzoek toe
                    string insertTradeRequestQuery = @"
                        INSERT INTO traderequest (status, user_id_requester, user_id_receiver)
                        VALUES ('Pending', @requesterId, @receiverId);
                        SELECT LAST_INSERT_ID();";

                    using (var cmd = new MySqlCommand(insertTradeRequestQuery, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@requesterId", requesterId);
                        cmd.Parameters.AddWithValue("@receiverId", receiverId);
                        tradeRequestId = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    // 2. Voeg de aangeboden speelgoedjes toe
                    string insertOfferedToyQuery = @"
                        INSERT INTO traderequest_offeredtoy (toy_id, traderequest_id)
                        VALUES (@toyId, @tradeRequestId)";
                    foreach (var toyId in offeredToyIds)
                    {
                        using (var cmd = new MySqlCommand(insertOfferedToyQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@toyId", toyId);
                            cmd.Parameters.AddWithValue("@tradeRequestId", tradeRequestId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // 3. Voeg de gevraagde speelgoedjes toe
                    string insertRequestedToyQuery = @"
                        INSERT INTO traderequest_requestedtoy (toy_id, traderequest_id)
                        VALUES (@toyId, @tradeRequestId)";
                    foreach (var toyId in requestedToyIds)
                    {
                        using (var cmd = new MySqlCommand(insertRequestedToyQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@toyId", toyId);
                            cmd.Parameters.AddWithValue("@tradeRequestId", tradeRequestId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // Commit de transactie als alles succesvol is
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // Rollback indien er iets misgaat
                    transaction.Rollback();
                    // Hier kun je de fout loggen of opnieuw throwen
                    throw new Exception("Fout bij het aanmaken van het ruilverzoek: " + ex.Message);
                }
            }
        }

        return tradeRequestId;
    }
}
