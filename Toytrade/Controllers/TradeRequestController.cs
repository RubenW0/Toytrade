using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;
using Microsoft.AspNetCore.Http;

namespace PresentationLayer.Controllers

{
    public class TradeRequestController : Controller
    {
        private readonly TradeRequestService _tradeRequestService;
        
        public TradeRequestController(TradeRequestService tradeRequestService)
        {
            _tradeRequestService = tradeRequestService;
        }




    }
}
