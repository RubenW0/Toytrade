using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;
using Microsoft.AspNetCore.Http;


namespace PresentationLayer.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _userService.Login(model.Username, model.Password);
            if (user != null)
            {
                HttpContext.Session.SetString("UserId", user.Id.ToString());
                HttpContext.Session.SetString("Username", user.Username);
                return RedirectToAction("Profile", "User");
            }

            ViewBag.Error = "Ongeldige gebruikersnaam of wachtwoord";
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "User"); 
        }

        public IActionResult Profile()
        {
            var model = new ProfileViewModel
            {
                Username = HttpContext.Session.GetString("Username")
            };

            if (string.IsNullOrEmpty(model.Username))
            {
                return RedirectToAction("Login", "User");
            }


            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userDTO = new UserDTO
            {
                Username = model.Username,
                Password = model.Password,
                Address = model.Address
            };

            _userService.Register(userDTO);

            return RedirectToAction("Login");
        }

    }
}

