using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.ViewModels;

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
            var toyDTOs = _toyService.GetAllToys();

            var toyViewModels = toyDTOs.Select(toy => new ToyViewModel
            {
                Id = toy.Id,
                Name = toy.Name,
                Image = toy.Image,
                Condition = toy.Condition,
                Username = toy.Username 
            }).ToList();

            var sortedToys = toyViewModels.OrderByDescending(t => t.Id).ToList();

            return View(sortedToys);
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
                ViewBag.Error = "Je moet ingelogd zijn om een speelgoed toe te voegen.";
                return View(model);
            }

            int userId = int.Parse(userIdString);

            var toyDTO = new ToyDTO
            {
                Name = model.Name,
                Image = model.Image,
                Condition = model.Condition,
                UserId = userId 
            };

            _toyService.AddToy(toyDTO);

            return RedirectToAction("Index");
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
                Image = toyDTO.Image,
                Condition = toyDTO.Condition
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

            var toyDTO = new ToyDTO
            {
                Id = model.Id,
                Name = model.Name,
                Image = model.Image,
                Condition = model.Condition
            };

            _toyService.UpdateToy(toyDTO);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _toyService.DeleteToy(id);
            return RedirectToAction("Index");
        }

        public IActionResult MyToys()
        {
            var userIdString = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Login", "User"); 
            }

            int userId = int.Parse(userIdString);

            var toyDTOs = _toyService.GetToysByUserId(userId);

            var toyViewModels = toyDTOs.Select(toy => new ToyViewModel
            {
                Id = toy.Id,
                Name = toy.Name,
                Image = toy.Image,
                Condition = toy.Condition,
                Username = toy.Username
            }).ToList();

            return View(toyViewModels);
        }

    }
}
