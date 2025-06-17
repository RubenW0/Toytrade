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
        private readonly ILogger<TradeRequestService> _logger;

        public TradeRequestService(ITradeRequestRepository tradeRequestRepository, IUserRepository userRepository, ILogger<TradeRequestService> logger)
        {
            _tradeRequestRepository = tradeRequestRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public List<TradeRequestDTO> GetTradeRequestsByUserId(int userId)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while retrieving trade requests for user {userId} in service.");
                throw;
            }
        }

        public int CreateTradeRequest(int requesterId, int receiverId, List<int> offeredToyIds, List<int> requestedToyIds)
        {
            try
            {
                return _tradeRequestRepository.CreateTradeRequest(requesterId, receiverId, offeredToyIds, requestedToyIds);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating trade request in service.");
                throw;
            }
        }

        public void RespondToTradeRequest(int requestId, bool accept)
        {
            try
            {
                var newStatus = accept ? "Accepted" : "Declined";
                _tradeRequestRepository.UpdateTradeRequestStatus(requestId, newStatus);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while responding to trade request with ID {requestId} in service.");
                throw;
            }
        }
    }
}
