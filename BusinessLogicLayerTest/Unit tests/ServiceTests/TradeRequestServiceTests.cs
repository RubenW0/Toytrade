using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services;
using BusinessLogicLayerTest.FakeTradeRequestRepos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Microsoft.Extensions.Logging.Abstractions;


namespace BusinessLogicLayerTest.ServiceTests
{
    [TestClass]
    public class TradeRequestServiceTests
    {
        [TestMethod]
        public void GetTradeRequestsByUserId_EmptyRepo_ReturnsEmptyList()
        {
            // Arrange
            var tradeRequestRepo = new FakeTradeRequestRepository_Empty();
            var userRepo = new FakeUserRepository_Static();
            var service = new TradeRequestService(tradeRequestRepo, userRepo, NullLogger<TradeRequestService>.Instance);

            // Act
            var result = service.GetTradeRequestsByUserId(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetTradeRequestsByUserId_RecordsRepo_ReturnsRequestsWithUsernamesAndToys()
        {
            // Arrange
            var tradeRequestRepo = new FakeTradeRequestRepository_Records();
            var userRepo = new FakeUserRepository_Static();
            var service = new TradeRequestService(tradeRequestRepo, userRepo, NullLogger<TradeRequestService>.Instance);

            // Act
            var result = service.GetTradeRequestsByUserId(1);

            // Assert
            Assert.AreEqual(2, result.Count);

            var first = result[0];
            Assert.AreEqual("User1", first.RequesterUsername);
            Assert.AreEqual("User2", first.ReceiverUsername);
            Assert.AreEqual(2, first.OfferedToys.Count);
            Assert.AreEqual(1, first.RequestedToys.Count);

            var second = result[1];
            Assert.AreEqual("User2", second.RequesterUsername);
            Assert.AreEqual("User1", second.ReceiverUsername);
            Assert.AreEqual(1, second.OfferedToys.Count);
            Assert.AreEqual(2, second.RequestedToys.Count);
        }

        [TestMethod]
        public void GetTradeRequestsByUserId_SingleRequestRepo_ReturnsOneRequestWithUsernamesAndToys()
        {
            // Arrange
            var tradeRequestRepo = new FakeTradeRequestRepository_SingleRequest();
            var userRepo = new FakeUserRepository_Static();
            var service = new TradeRequestService(tradeRequestRepo, userRepo, NullLogger<TradeRequestService>.Instance);

            // Act
            var result = service.GetTradeRequestsByUserId(1);

            // Assert
            Assert.AreEqual(1, result.Count);

            var request = result[0];
            Assert.AreEqual("User1", request.RequesterUsername);
            Assert.AreEqual("User2", request.ReceiverUsername);
            Assert.AreEqual(1, request.OfferedToys.Count);
            Assert.AreEqual(1, request.RequestedToys.Count);
        }

        [TestMethod]
        public void CreateTradeRequest_RecordsRepo_ReturnsCreatedRequestId()
        {
            // Arrange
            var tradeRequestRepo = new FakeTradeRequestRepository_Records();
            var userRepo = new FakeUserRepository_Static();
            var service = new TradeRequestService(tradeRequestRepo, userRepo, NullLogger<TradeRequestService>.Instance);

            // Act
            int newId = service.CreateTradeRequest(1, 2, new List<int> { 101 }, new List<int> { 201 });

            // Assert
            Assert.AreEqual(99, newId);
        }

        [TestMethod]
        public void RespondToTradeRequest_RecordsRepo_UpdatesStatusAccepted()
        {
            // Arrange
            var tradeRequestRepo = new FakeTradeRequestRepository_Records();
            var userRepo = new FakeUserRepository_Static();
            var service = new TradeRequestService(tradeRequestRepo, userRepo, NullLogger<TradeRequestService>.Instance);

            // Act
            service.RespondToTradeRequest(5, true);

            // Assert
            Assert.AreEqual(5, tradeRequestRepo.LastUpdatedRequestId);
            Assert.AreEqual("Accepted", tradeRequestRepo.LastStatus);
        }

        [TestMethod]
        public void RespondToTradeRequest_RecordsRepo_UpdatesStatusDeclined()
        {
            // Arrange
            var tradeRequestRepo = new FakeTradeRequestRepository_Records();
            var userRepo = new FakeUserRepository_Static();
            var service = new TradeRequestService(tradeRequestRepo, userRepo, NullLogger<TradeRequestService>.Instance);

            // Act
            service.RespondToTradeRequest(6, false);

            // Assert
            Assert.AreEqual(6, tradeRequestRepo.LastUpdatedRequestId);
            Assert.AreEqual("Declined", tradeRequestRepo.LastStatus);
        }
    }
}
