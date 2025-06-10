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

        public TradeRequestController(TradeRequestService tradeRequestService, ToyService toyService)
        {
            _tradeRequestService = tradeRequestService;
            _toyService = toyService;
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
            var selectedRequestedToyIds = new List<int> { chosenToyId };

            var receiverToys = _toyService.GetAllToys()
                .Where(toy => toy.UserId == receiverId)
                .Select(toy => new ToyViewModel
                {
                    Id = toy.Id,
                    Name = toy.Name,
                    Image = toy.Image,
                    Condition = Enum.TryParse<ToyCondition>(toy.Condition, out var parsedCondition) ? parsedCondition : ToyCondition.Used,
                })
                .OrderByDescending(toy => selectedRequestedToyIds.Contains(toy.Id))
                .ToList();

            var myToys = _toyService.GetAllToys()
                .Where(t => t.UserId == currentUserId)
                .Select(t => new ToyViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    Image = t.Image,
                    Condition = Enum.TryParse<ToyCondition>(t.Condition, out var parsedCondition) ? parsedCondition : ToyCondition.Used,
                })
                .ToList();

            var model = new TradeRequestCreateViewModel
            {
                ReceiverId = receiverId,
                RequestedToyIds = selectedRequestedToyIds,
                ReceiverToys = receiverToys,
                MyToys = myToys
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TradeRequestCreateViewModel model)
        {
            string? userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Login", "User");
            }

            int requesterId = int.Parse(userIdString);
            int receiverId = model.ReceiverId;

            if (!ModelState.IsValid ||
                ((model.OfferedToyIds == null || !model.OfferedToyIds.Any()) &&
                 (model.RequestedToyIds == null || !model.RequestedToyIds.Any())))
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Select at least one offered and/or requested toy.");
                }

                model.MyToys = _toyService.GetAllToys()
                    .Where(t => t.UserId == requesterId)
                    .Select(t => new ToyViewModel
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Image = t.Image,
                        Condition = Enum.TryParse<ToyCondition>(t.Condition, out var parsedCondition) ? parsedCondition : ToyCondition.Used,
                    })
                    .OrderByDescending(t => model.OfferedToyIds?.Contains(t.Id) ?? false)
                    .ToList();

                model.ReceiverToys = _toyService.GetAllToys()
                    .Where(t => t.UserId == receiverId)
                    .Select(t => new ToyViewModel
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Image = t.Image,
                        Condition = Enum.TryParse<ToyCondition>(t.Condition, out var parsedCondition) ? parsedCondition : ToyCondition.Used,
                    })
                    .OrderByDescending(t => model.RequestedToyIds?.Contains(t.Id) ?? false)
                    .ToList();

                return View(model);
            }

            int tradeRequestId = _tradeRequestService.CreateTradeRequest(
                requesterId, receiverId,
                model.OfferedToyIds ?? new List<int>(),
                model.RequestedToyIds ?? new List<int>());

            return RedirectToAction("MyRequests");
        }
    }
}
