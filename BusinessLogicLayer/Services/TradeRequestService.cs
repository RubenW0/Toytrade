using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer.Services
{
    public class TradeRequestService
    {
        private readonly ITradeRequestRepository _tradeRequestRepository;
        private readonly IUserRepository _userRepository;

        public TradeRequestService(ITradeRequestRepository tradeRequestRepository, IUserRepository userRepository)
        {
            _tradeRequestRepository = tradeRequestRepository;
            _userRepository = userRepository;
        }

        public List<TradeRequestDTO> GetTradeRequestsByUserId(int userId)
        {
            var requests = _tradeRequestRepository.GetTradeRequestsByUserId(userId);

            foreach (var req in requests)
            {
                req.RequesterUsername = _userRepository.GetUsernameById(req.RequesterId);
                req.ReceiverUsername = _userRepository.GetUsernameById(req.ReceiverId);

                req.OfferedToys = _tradeRequestRepository.GetOfferedToysByTradeRequestId(req.Id);
                req.RequestedToys = _tradeRequestRepository.GetRequestedToysByTradeRequestId(req.Id);
            }

            return requests;
        }

        public int CreateTradeRequest(int requesterId, int receiverId, List<int> offeredToyIds, List<int> requestedToyIds)
        {
            return _tradeRequestRepository.CreateTradeRequest(requesterId, receiverId, offeredToyIds, requestedToyIds);
        }

        public void RespondToTradeRequest(int requestId, bool accept)
        {
            var newStatus = accept ? "Accepted" : "Declined";
            _tradeRequestRepository.UpdateTradeRequestStatus(requestId, newStatus);
        }
    }
}
