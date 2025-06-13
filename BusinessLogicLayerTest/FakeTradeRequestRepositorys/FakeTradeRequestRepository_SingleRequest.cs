using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.IRepositories;
using System.Collections.Generic;

namespace BusinessLogicLayerTest.FakeTradeRequestRepos
{
    public class FakeTradeRequestRepository_SingleRequest : ITradeRequestRepository
    {
        public int CreatedRequestId { get; private set; }
        public int LastUpdatedRequestId { get; private set; }
        public string LastStatus { get; private set; }

        public List<TradeRequestDTO> GetTradeRequestsByUserId(int userId)
        {
            return new List<TradeRequestDTO>
            {
                new TradeRequestDTO
                {
                    Id = 1,
                    RequesterId = 1,
                    ReceiverId = 2
                }
            };
        }

        public List<ToyDTO> GetOfferedToysByTradeRequestId(int tradeRequestId)
        {
            return new List<ToyDTO>
            {
                new ToyDTO { Id = 101, Name = "SingleOfferedToy", UserId = 1 }
            };
        }

        public List<ToyDTO> GetRequestedToysByTradeRequestId(int tradeRequestId)
        {
            return new List<ToyDTO>
            {
                new ToyDTO { Id = 201, Name = "SingleRequestedToy", UserId = 2 }
            };
        }

        public int CreateTradeRequest(int requesterId, int receiverId, List<int> offeredToyIds, List<int> requestedToyIds)
        {
            CreatedRequestId = 1;
            return CreatedRequestId;
        }

        public void UpdateTradeRequestStatus(int tradeRequestId, string newStatus)
        {
            LastUpdatedRequestId = tradeRequestId;
            LastStatus = newStatus;
        }
    }
}
