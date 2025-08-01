﻿using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Enums;
using BusinessLogicLayer.IRepositories;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

public class TradeRequestRepository : ITradeRequestRepository
{
    private readonly string _connectionString;

    public TradeRequestRepository(IConfiguration configuration, ILogger<TradeRequestRepository> logger)
    {
        _connectionString = configuration.GetConnectionString("MySqlConnection");
    }

    public List<ToyDTO> GetOfferedToysByTradeRequestId(int tradeRequestId)
    {
        try
        {
            var toys = new List<ToyDTO>();
            string query =
                @"SELECT t.id, t.name, t.image_path    
             FROM toy t    
             JOIN traderequest_offeredtoy ot ON t.id = ot.toy_id    
             WHERE ot.traderequest_id = @tradeRequestId";

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@tradeRequestId", tradeRequestId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                toys.Add(new ToyDTO
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    ImagePath = reader.GetString("image_path")
                });
            }

            return toys;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public List<ToyDTO> GetRequestedToysByTradeRequestId(int tradeRequestId)
    {
        try
        {
            var toys = new List<ToyDTO>();
            string query = @"SELECT t.id, t.name, t.image_path    
                      FROM toy t    
                      JOIN traderequest_requestedtoy rt ON t.id = rt.toy_id    
                      WHERE rt.traderequest_id = @tradeRequestId";

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@tradeRequestId", tradeRequestId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                toys.Add(new ToyDTO
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    ImagePath = reader.GetString("image_path")
                });
            }

            return toys;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public List<TradeRequestDTO> GetTradeRequestsByUserId(int userId)
    {
        try
        {
            var requests = new List<TradeRequestDTO>();
            string query = @"SELECT id, status, user_id_requester, user_id_receiver, created_at, responded_at    
                       FROM traderequest    
                       WHERE user_id_requester = @userId OR user_id_receiver = @userId";

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@userId", userId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                requests.Add(new TradeRequestDTO
                {
                    Id = reader.GetInt32("id"),
                    Status = Enum.Parse<TradeRequestStatus>(reader.GetString("status")),
                    RequesterId = reader.GetInt32("user_id_requester"),
                    ReceiverId = reader.GetInt32("user_id_receiver"),
                    CreatedAt = reader.GetDateTime("created_at"),
                    RespondedAt = reader.IsDBNull(reader.GetOrdinal("responded_at"))
                                  ? (DateTime?)null
                                  : reader.GetDateTime("responded_at")
                });
            }

            return requests;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public int CreateTradeRequest(int requesterId, int receiverId, List<int> offeredToyIds, List<int> requestedToyIds)
    {
        try
        {
            int tradeRequestId = 0;
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

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

            string insertOfferedToyQuery = @"    
             INSERT INTO traderequest_offeredtoy (toy_id, traderequest_id)    
             VALUES (@toyId, @tradeRequestId)";
            foreach (var toyId in offeredToyIds)
            {
                using var cmd = new MySqlCommand(insertOfferedToyQuery, connection, transaction);
                cmd.Parameters.AddWithValue("@toyId", toyId);
                cmd.Parameters.AddWithValue("@tradeRequestId", tradeRequestId);
                cmd.ExecuteNonQuery();
            }

            string insertRequestedToyQuery = @"    
             INSERT INTO traderequest_requestedtoy (toy_id, traderequest_id)    
             VALUES (@toyId, @tradeRequestId)";
            foreach (var toyId in requestedToyIds)
            {
                using var cmd = new MySqlCommand(insertRequestedToyQuery, connection, transaction);
                cmd.Parameters.AddWithValue("@toyId", toyId);
                cmd.Parameters.AddWithValue("@tradeRequestId", tradeRequestId);
                cmd.ExecuteNonQuery();
            }

            transaction.Commit();
            return tradeRequestId;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void UpdateTradeRequestStatus(int tradeRequestId, string newStatus)
    {
        try
        {
            string query = @"UPDATE traderequest   
               SET status = @newStatus, responded_at = NOW()   
               WHERE id = @tradeRequestId";

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@newStatus", newStatus);
            cmd.Parameters.AddWithValue("@tradeRequestId", tradeRequestId);
            cmd.ExecuteNonQuery();
        }
        catch (Exception)
        {
            throw;
        }
    }
}
