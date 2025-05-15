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


            
     
 }
