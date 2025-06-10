using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;
using Microsoft.AspNetCore.Http;
using PresentationLayer.Models.Enums;

namespace PresentationLayer.Controllers

{
    public class TradeRequestController : Controller
    {
        private readonly TradeRequestService _tradeRequestService;
        private readonly ToyService _toyService;
        private readonly UserService _userService;

        public TradeRequestController(TradeRequestService tradeRequestService, ToyService toyService, UserService userService)
        {
            _tradeRequestService = tradeRequestService;
            _toyService = toyService;
            _userService = userService;
        }

        public IActionResult MyRequests()
        {
            string? userIdString = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Login", "User");
            }

            int userId = int.Parse(userIdString);
            var toyDTOs = _toyService.GetAllToys().ToList(); 

            var requests = _tradeRequestService.GetTradeRequestsByUserId(userId);

            var viewModelList = requests.Select(request => new TradeRequestViewModel
            {
                Status = request.Status,
                RequesterUsername = request.RequesterUsername,
                ReceiverUsername = request.ReceiverUsername,
                OfferedToys = request.OfferedToys.Select(toy => new ToyViewModel
                {
                    Id = toy.Id,
                    Name = toy.Name,
                    Image = toy.Image,
                    Condition = Enum.TryParse<ToyCondition>(toy.Condition, out var parsedCondition) ? parsedCondition : ToyCondition.Used,
                    Username = toyDTOs.FirstOrDefault(t => t.Id == toy.Id)?.Username
                }).ToList(),
                RequestedToys = request.RequestedToys.Select(toy => new ToyViewModel
                {
                    Id = toy.Id,
                    Name = toy.Name,
                    Image = toy.Image,
                    Condition = Enum.TryParse<ToyCondition>(toy.Condition, out var parsedCondition) ? parsedCondition : ToyCondition.Used,
                    Username = toyDTOs.FirstOrDefault(t => t.Id == toy.Id)?.Username
                }).ToList()
            }).ToList();

            return View(viewModelList);
        }

        [HttpGet]
        public IActionResult Create(int chosenToyId)
        {
            string? userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Login", "User");
            }

            int currentUserId = int.Parse(userIdString);

            var chosenToy = _toyService.GetToyById(chosenToyId);
            if (chosenToy == null)
            {
                return NotFound();
            }

            int receiverId = chosenToy.UserId;

            var receiverToys = _toyService.GetAllToys()
                .Where(toy => toy.UserId == receiverId)
                .Select(toy => new ToyViewModel
                {
                    Id = toy.Id,
                    Name = toy.Name,
                    Image = toy.Image,
                    Condition = Enum.TryParse<ToyCondition>(toy.Condition, out var parsedCondition) ? parsedCondition : ToyCondition.Used,
                }).ToList();

            var selectedRequestedToyIds = new List<int> { chosenToyId };

            var model = new TradeRequestCreateViewModel
            {
                ReceiverId = receiverId, 
                RequestedToyIds = selectedRequestedToyIds, 
                ReceiverToys = receiverToys, 
                MyToys = _toyService.GetAllToys().Where(t => t.UserId == currentUserId).Select(t => new ToyViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    Image = t.Image,
                    Condition = Enum.TryParse<ToyCondition>(t.Condition, out var parsedCondition) ? parsedCondition : ToyCondition.Used,
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TradeRequestCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string? userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Login", "User");
            }

            int requesterId = int.Parse(userIdString);
            int receiverId = model.ReceiverId;

            if ((model.OfferedToyIds == null || !model.OfferedToyIds.Any()) &&
                (model.RequestedToyIds == null || !model.RequestedToyIds.Any()))
            {
                ModelState.AddModelError("", "Selecteer minimaal één aangeboden en/of gevraagde toy.");
                return View(model);
            }

            int tradeRequestId = _tradeRequestService.CreateTradeRequest(
                requesterId, receiverId, model.OfferedToyIds ?? new List<int>(), model.RequestedToyIds ?? new List<int>());

            return RedirectToAction("MyRequests");
        }


    }
}
