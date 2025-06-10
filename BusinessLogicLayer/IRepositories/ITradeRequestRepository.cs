using BusinessLogicLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.IRepositories
{
    public interface ITradeRequestRepository
    {
        List<ToyDTO> GetOfferedToysByTradeRequestId(int tradeRequestId);
        List<ToyDTO> GetRequestedToysByTradeRequestId(int tradeRequestId);
        List<TradeRequestDTO> GetTradeRequestsByUserId(int userId);
        public int CreateTradeRequest(int requesterId, int receiverId, List<int> offeredToyIds, List<int> requestedToyIds);
    }
}
