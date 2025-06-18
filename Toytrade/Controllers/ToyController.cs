using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models.Enums;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers
{
    public class ToyController : Controller
    {
        private readonly ToyService _toyService;

        public ToyController(ToyService toyService)
        {
            _toyService = toyService;
        }

        public IActionResult Index()
        {
            try
            {
                var toyDTOs = _toyService.GetAllToys().ToList();

                var toyViewModels = toyDTOs.Select(toy => new ToyViewModel
                {
                    Id = toy.Id,
                    Name = toy.Name,
                    Image = toy.ImagePath,
                    Condition = Enum.TryParse<ToyCondition>(toy.Condition, out var parsedCondition) ? parsedCondition : ToyCondition.Used,
                    Username = toy.Username
                }).OrderByDescending(t => t.Id).ToList();

                return View(toyViewModels);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An error occurred while loading the toys.";
                Console.WriteLine(ex);
                return View(new List<ToyViewModel>());
            }
        }

        [HttpGet]
        public IActionResult AddToy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddToy(ToyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                foreach (var error in errors)
                {
                    Console.WriteLine(error);
                }
                return View(model);
            }

            var userIdString = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userIdString))
            {
                ViewBag.Error = "You must be logged in to add a toy.";
                return View(model);
            }

            try
            {
                int userId = int.Parse(userIdString);

                var toyDTO = new ToyDTO
                {
                    Name = model.Name,
                    Condition = model.Condition.ToString(),
                    UserId = userId,
                    ImageFile = model.ImageFile
                };

                _toyService.AddToy(toyDTO);

                return RedirectToAction("MyToys");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while adding toy: " + ex.ToString());

                ViewBag.Error = "An error occurred while adding the toy. Please try again later.";
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var toyDTO = _toyService.GetAllToys().FirstOrDefault(t => t.Id == id);

            if (toyDTO == null)
            {
                return NotFound();
            }

            var toyViewModel = new ToyViewModel
            {
                Id = toyDTO.Id,
                Name = toyDTO.Name,
                Condition = Enum.Parse<ToyCondition>(toyDTO.Condition),
                Image = toyDTO.ImagePath
            };

            return View(toyViewModel);
        }

        [HttpPost]
        public IActionResult Edit(ToyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var toyDTO = new ToyDTO
                {
                    Id = model.Id,
                    Name = model.Name,
                    Condition = model.Condition.ToString(),
                    ImageFile = model.ImageFile
                };

                _toyService.UpdateToy(toyDTO);
                return RedirectToAction("MyToys");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An error occurred while editing the toy.";
                Console.WriteLine(ex);
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _toyService.DeleteToy(id);
            return RedirectToAction("Index");
        }

        public IActionResult MyToys()
        {
            string? userIdString = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Login", "User");
            }

            try
            {
                int userId = int.Parse(userIdString);
                var toyDTOs = _toyService.GetToysByUserId(userId);

                var toyViewModels = toyDTOs.Select(toy => new ToyViewModel
                {
                    Id = toy.Id,
                    Name = toy.Name,
                    Image = toy.ImagePath,
                    Condition = Enum.TryParse<ToyCondition>(toy.Condition, out var parsedCondition) ? parsedCondition : ToyCondition.Used,
                    Username = toy.Username
                }).OrderByDescending(t => t.Id).ToList();

                return View(toyViewModels);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An error occurred while retrieving your toys.";
                Console.WriteLine(ex);
                return View(new List<ToyViewModel>());
            }
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var toyDTO = _toyService.GetAllToys().FirstOrDefault(t => t.Id == id);

            if (toyDTO == null)
            {
                return View("ToyNotFound", id);
            }

            var toyViewModel = new ToyViewModel
            {
                Id = toyDTO.Id,
                Name = toyDTO.Name,
                Condition = Enum.TryParse<ToyCondition>(toyDTO.Condition, out var parsedCondition) ? parsedCondition : ToyCondition.Used,
                Image = toyDTO.ImagePath,
                Username = toyDTO.Username,
                UserId = toyDTO.UserId
            };

            return View("ToyDetails", toyViewModel);
        }

    }
}
