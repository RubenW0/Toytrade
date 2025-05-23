﻿using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models.Enums;
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
            var userIdString = HttpContext.Session.GetString("UserId");
            int? userId = string.IsNullOrEmpty(userIdString) ? null : int.Parse(userIdString);

            var toyDTOs = _toyService.GetAllToys()
                .Where(toy => toy.UserId != userId)
                .ToList();

            var toyViewModels = toyDTOs.Select(toy => new ToyViewModel
            {
                Id = toy.Id,
                Name = toy.Name,
                Image = toy.ImagePath,
                Condition = Enum.TryParse<ToyCondition>(toy.Condition, out var parsedCondition) ? parsedCondition : ToyCondition.Used,
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
                Condition = model.Condition.ToString(), 
                UserId = userId,
                ImageFile = model.ImageFile
            };

            _toyService.AddToy(toyDTO);

            return RedirectToAction("MyToys");
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

            int userId = int.Parse(userIdString);

            var toyDTOs = _toyService.GetToysByUserId(userId);


            var toyViewModels = toyDTOs.Select(toy => new ToyViewModel
            {
                Id = toy.Id,
                Name = toy.Name,
                Image = toy.ImagePath,
                Condition = Enum.TryParse<ToyCondition>(toy.Condition, out var parsedCondition) ? parsedCondition : ToyCondition.Used,
                Username = toy.Username
            }).ToList();

            var sortedToys = toyViewModels.OrderByDescending(t => t.Id).ToList();

            return View(sortedToys);
        }

    }
}
