using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.IRepositories;
using BusinessLogicLayer.Services;
using DataAccessLayer.Repositorys;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using System;
using System.Linq;
using Microsoft.Extensions.Configuration.Json;



namespace BusinessLogicLayerTest.IntegrationTests
{
    [TestClass]
    [DoNotParallelize]
    public class ToyServiceIntegrationTests
    {
        private ServiceProvider _serviceProvider;
        private ToyService _toyService;
        private string _connectionString;

        [TestInitialize]
        public void Setup()
        {
            var services = new ServiceCollection();

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Test.json")
                .Build();

            _connectionString = config.GetConnectionString("MySqlConnection");

            services.AddSingleton<IConfiguration>(config);
            services.AddLogging();

            services.AddScoped<IToyRepository, ToyRepository>();

            services.AddScoped<IUserRepository, FakeUserRepository_Static>(); 

            services.AddScoped<ToyService>();

            _serviceProvider = services.BuildServiceProvider();

            _toyService = _serviceProvider.GetRequiredService<ToyService>();

            Console.WriteLine($"Aantal toys vóór ClearToyTable:");
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using var cmd = new MySqlCommand("SELECT COUNT(*) FROM Toy", connection);
                var count = Convert.ToInt32(cmd.ExecuteScalar());
                Console.WriteLine(count);
            }

            Console.WriteLine($"Using connection string: {_connectionString}");

            ClearToyTable();
        }

        private void ClearToyTable()
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var cmd = new MySqlCommand("DELETE FROM Toy", connection);
            cmd.ExecuteNonQuery();

            using var cmdCheck = new MySqlCommand("SELECT COUNT(*) FROM Toy", connection);
            var count = Convert.ToInt32(cmdCheck.ExecuteScalar());
            Console.WriteLine($"Aantal toys na clear: {count}");
        }

        [TestMethod]
        public void AddToy_ShouldAddToyToDatabase()
        {
            var toy = new ToyDTO
            {
                Name = "Test Toy",
                Condition = "Nieuw",
                UserId = 1 
            };

            _toyService.AddToy(toy);
            var result = _toyService.GetAllToys();

            Assert.IsTrue(result.Any(t => t.Name == "Test Toy"));
        }

        [TestMethod]
        public void DeleteToy_ShouldRemoveToyFromDatabase()
        {
            var toy = new ToyDTO
            {
                Name = "To Be Deleted",
                Condition = "Gebruikt",
                UserId = 1
            };

            _toyService.AddToy(toy);
            var allToys = _toyService.GetAllToys();
            var toyToDelete = allToys.First(t => t.Name == "To Be Deleted");

            _toyService.DeleteToy(toyToDelete.Id);
            var result = _toyService.GetAllToys();

            Assert.IsFalse(result.Any(t => t.Id == toyToDelete.Id));
        }

        [TestMethod]
        public void GetAllToys_ShouldReturnMultipleToys()
        {
            _toyService.AddToy(new ToyDTO { Name = "Toy 1", Condition = "Nieuw", UserId = 1 });
            _toyService.AddToy(new ToyDTO { Name = "Toy 2", Condition = "Gebruikt", UserId = 1 });

            var result = _toyService.GetAllToys();

            Assert.AreEqual(2, result.Count);
        }

        [TestCleanup]
        public void Cleanup()
        {
            ClearToyTable();
            _serviceProvider.Dispose();
        }
    }
}