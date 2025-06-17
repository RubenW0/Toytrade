using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.IRepositories;
using System.Collections.Generic;

namespace BusinessLogicLayerTest.FakeTradeRequestRepos
{
    public class FakeTradeRequestRepository_Records : ITradeRequestRepository
    {
        public int CreatedRequestId { get; private set; }
        public int LastUpdatedRequestId { get; private set; }
        public string LastStatus { get; private set; }

        public List<TradeRequestDTO> GetTradeRequestsByUserId(int userId) => new List<TradeRequestDTO>
        {
            new TradeRequestDTO
            {
                Id = 1,
                RequesterId = 1,
                ReceiverId = 2
            },
            new TradeRequestDTO
            {
                Id = 2,
                RequesterId = 2,
                ReceiverId = 1
            }
        };

        public List<ToyDTO> GetOfferedToysByTradeRequestId(int tradeRequestId)
        {
            return tradeRequestId switch
            {
                1 => new List<ToyDTO>
                {
                    new ToyDTO { Id = 101, Name = "ToyA", UserId = 1 },
                    new ToyDTO { Id = 102, Name = "ToyB", UserId = 1 }
                },
                2 => new List<ToyDTO>
                {
                    new ToyDTO { Id = 201, Name = "ToyC", UserId = 2 }
                },
                _ => new List<ToyDTO>()
            };
        }

        public List<ToyDTO> GetRequestedToysByTradeRequestId(int tradeRequestId)
        {
            return tradeRequestId switch
            {
                1 => new List<ToyDTO>
                {
                    new ToyDTO { Id = 301, Name = "ToyX", UserId = 2 }
                },
                2 => new List<ToyDTO>
                {
                    new ToyDTO { Id = 401, Name = "ToyY", UserId = 1 },
                    new ToyDTO { Id = 402, Name = "ToyZ", UserId = 1 }
                },
                _ => new List<ToyDTO>()
            };
        }

        public int CreateTradeRequest(int requesterId, int receiverId, List<int> offeredToyIds, List<int> requestedToyIds)
        {
            CreatedRequestId = 99; 
            return CreatedRequestId;
        }

        public void UpdateTradeRequestStatus(int requestId, string status)
        {
            LastUpdatedRequestId = requestId;
            LastStatus = status;
        }
    }
}
