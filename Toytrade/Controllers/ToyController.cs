using BusinessLogicLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class ToyController : Controller
    {
        private readonly ToyService _toyService;

        public ToyController(ToyService toyService)
        {
            _toyService = toyService;
        }

        public IActionResult Toys()
        {
            var toys = _toyService.GetAllToys();
            return View(toys);
        }
    }
}
