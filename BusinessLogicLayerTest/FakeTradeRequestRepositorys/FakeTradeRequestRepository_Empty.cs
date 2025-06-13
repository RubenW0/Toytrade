using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.IRepositories;
using System.Collections.Generic;

namespace BusinessLogicLayerTest.FakeTradeRequestRepos
{
    public class FakeTradeRequestRepository_Empty : ITradeRequestRepository
    {
        public List<TradeRequestDTO> GetTradeRequestsByUserId(int userId)
        {
            return new List<TradeRequestDTO>();
        }

        public List<ToyDTO> GetOfferedToysByTradeRequestId(int tradeRequestId)
        {
            return new List<ToyDTO>();
        }

        public List<ToyDTO> GetRequestedToysByTradeRequestId(int tradeRequestId)
        {
            return new List<ToyDTO>();
        }

        public int CreateTradeRequest(int requesterId, int receiverId, List<int> offeredToyIds, List<int> requestedToyIds)
        {
            return 0; 
        }

        public void UpdateTradeRequestStatus(int tradeRequestId, string newStatus)
        {
        }
    }
}
