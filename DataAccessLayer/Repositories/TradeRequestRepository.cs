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

    public void CreateTradeRequest(TradeRequestDTO request)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    // 1. Insert into TradeRequest
                    var query = "INSERT INTO TradeRequest (status, user_id_requester, user_id_receiver) VALUES (@status, @requester, @receiver); SELECT LAST_INSERT_ID();";
                    int tradeRequestId;
                    using (var cmd = new MySqlCommand(query, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@status", request.Status);
                        cmd.Parameters.AddWithValue("@requester", request.UserIdRequester);
                        cmd.Parameters.AddWithValue("@receiver", request.UserIdReceiver);
                        tradeRequestId = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    // 2. Insert into TradeRequest_OfferedToy
                    foreach (var toyId in request.OfferedToyIds)
                    {
                        var offerQuery = "INSERT INTO TradeRequest_OfferedToy (toy_id, traderequest_id) VALUES (@toyId, @requestId)";
                        using (var cmd = new MySqlCommand(offerQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@toyId", toyId);
                            cmd.Parameters.AddWithValue("@requestId", tradeRequestId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // 3. Insert into TradeRequest_RequestedToy
                    foreach (var toyId in request.RequestedToyIds)
                    {
                        var reqQuery = "INSERT INTO TradeRequest_RequestedToy (toy_id, traderequest_id) VALUES (@toyId, @requestId)";
                        using (var cmd = new MySqlCommand(reqQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@toyId", toyId);
                            cmd.Parameters.AddWithValue("@requestId", tradeRequestId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Fout bij het aanmaken van ruilverzoek", ex);
                }
            }
        }
    }
}
