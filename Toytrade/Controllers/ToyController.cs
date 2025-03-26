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

            return View(toyViewModels);
        }

        [HttpGet]
        public IActionResult AddToy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddToy(ToyViewModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(model);
            //}

            var toyDTO = new ToyDTO
            {
                Name = model.Name,
                Image = model.Image,
                Condition = model.Condition,
                UserId = 1 // TEMPORARY, replace with user session id
            };

            _toyService.AddToy(toyDTO);

            return RedirectToAction("Index");
        }

    }
}
