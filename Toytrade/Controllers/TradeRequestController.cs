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

        public IActionResult MyRequests()
        {
            string? userIdString = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Login", "User");
            }

            int userId = int.Parse(userIdString);

            var requests = _tradeRequestService.GetTradeRequestsByUserId(userId);
            return View(requests);
        }



    }
}
