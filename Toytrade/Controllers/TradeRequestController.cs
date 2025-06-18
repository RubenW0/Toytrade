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
        private readonly ILogger<TradeRequestController> _logger;

        public TradeRequestController(TradeRequestService tradeRequestService, ToyService toyService, ILogger<TradeRequestController> logger)
        {
            _tradeRequestService = tradeRequestService;
            _toyService = toyService;
            _logger = logger;
        }

        public IActionResult MyRequests()
        {
            try
            {
                string? userIdString = HttpContext.Session.GetString("UserId");
                string? currentUsername = HttpContext.Session.GetString("Username");

                if (string.IsNullOrEmpty(userIdString) || string.IsNullOrEmpty(currentUsername))
                {
                    return RedirectToAction("Login", "User");
                }

                int userId = int.Parse(userIdString);
                var toyDTOs = _toyService.GetAllToys().ToList();
                var requests = _tradeRequestService.GetTradeRequestsByUserId(userId);

                var viewModelList = requests
                    .OrderByDescending(r => r.RespondedAt ?? r.CreatedAt)
                    .Select(request => new TradeRequestViewModel
                    {
                        Id = request.Id,
                        Status = request.Status.ToString(),
                        RequesterUsername = request.RequesterUsername,
                        ReceiverUsername = request.ReceiverUsername,
                        Username = currentUsername,
                        CreatedAt = request.CreatedAt,
                        RespondedAt = request.RespondedAt,
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
            catch (Exception ex)
            {
                LogErrorWithMethodName(ex, "An error occurred while retrieving trade requests.");
                ViewBag.Error = "An error occurred while retrieving your trade requests.";
                return View(new List<TradeRequestViewModel>());
            }
        }

        [HttpGet]
        public IActionResult Create(int chosenToyId)
        {
            try
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

                if (chosenToy.UserId == currentUserId)
                {
                    ViewBag.Error = "You cannot trade with yourself.";
                    return RedirectToAction("Details", "Toy", new { id = chosenToyId });
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
            catch (Exception ex)
            {
                LogErrorWithMethodName(ex, "An error occurred while preparing the trade request.");
                ViewBag.Error = "An error occurred while preparing the trade request.";
                return View(new TradeRequestCreateViewModel());
            }
        }

        [HttpPost]
        public IActionResult Create(TradeRequestCreateViewModel model)
        {
            try
            {
                string? userIdString = HttpContext.Session.GetString("UserId");
                if (string.IsNullOrEmpty(userIdString))
                {
                    return RedirectToAction("Login", "User");
                }

                int requesterId = int.Parse(userIdString);
                int receiverId = model.ReceiverId;

                if (model.OfferedToyIds == null || !model.OfferedToyIds.Any())
                {
                    ModelState.AddModelError("OfferedToyIds", "Please select at least one toy to offer.");
                }

                if (model.RequestedToyIds == null || !model.RequestedToyIds.Any())
                {
                    ModelState.AddModelError("RequestedToyIds", "Please select at least one toy to request.");
                }

                if (!ModelState.IsValid)
                {
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
            catch (Exception ex)
            {
                LogErrorWithMethodName(ex, "An error occurred while creating the trade request.");
                ViewBag.Error = "An error occurred while creating the trade request.";
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult Respond(int requestId, string response)
        {
            try
            {
                string? userIdString = HttpContext.Session.GetString("UserId");
                if (string.IsNullOrEmpty(userIdString))
                {
                    return RedirectToAction("Login", "User");
                }

                bool accept = response == "accept";
                _tradeRequestService.RespondToTradeRequest(requestId, accept);

                return RedirectToAction("MyRequests");
            }
            catch (Exception ex)
            {
                LogErrorWithMethodName(ex, $"An error occurred while responding to the trade request with ID {requestId}.");
                ViewBag.Error = "An error occurred while responding to the trade request.";
                return RedirectToAction("MyRequests");
            }
        }

        private void LogErrorWithMethodName(Exception ex, string? extraMessage = null, [System.Runtime.CompilerServices.CallerMemberName] string callerName = "")
        {
            var msg = $"Exception in {callerName}";
            if (!string.IsNullOrEmpty(extraMessage))
                msg += $": {extraMessage}";

            _logger.LogError(ex, msg);
        }
    }
}
