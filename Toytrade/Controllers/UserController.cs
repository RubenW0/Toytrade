using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;
using Microsoft.AspNetCore.Http;
using PresentationLayer.Models.Enums;


namespace PresentationLayer.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly ToyService _toyService;

        public UserController(UserService userService, ToyService toyService)
        {
            _userService = userService;
            _toyService = toyService;
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
                ViewBag.Error = "Invalid input. Please check your username and password.";
                return View(model);
            }

            try
            {
                var user = _userService.Login(model.Username, model.Password);
                if (user != null)
                {
                    HttpContext.Session.SetString("UserId", user.Id.ToString());
                    HttpContext.Session.SetString("Username", user.Username);
                    return RedirectToAction("Profile", "User");
                }

                ViewBag.Error = "Invalid username or password.";
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An error occurred while processing your request. Please try again later.";
                Console.WriteLine(ex);
                return View(model);
            }
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
            {
                ViewBag.Error = "Invalid input. Please check the required fields.";
                return View(model);
            }

            try
            {
                var userDTO = new UserDTO
                {
                    Username = model.Username,
                    Password = model.Password,
                    Address = model.Address
                };

                _userService.Register(userDTO);
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An error occurred while registering the user. Please try again later.";
                Console.WriteLine(ex);
                return View(model);
            }
        }

        public IActionResult PublicProfile(int userId)
        {
            try
            {
                var user = _userService.GetUserById(userId);
                if (user == null)
                {
                    return NotFound();
                }

                var toys = _toyService.GetToysByUserId(userId);

                var model = new PublicProfileViewModel
                {
                    Username = user.Username,
                    Toys = toys.Select(toy => new ToyViewModel
                    {
                        Id = toy.Id,
                        Name = toy.Name,
                        Image = toy.ImagePath,
                        Condition = Enum.TryParse<ToyCondition>(toy.Condition, out var parsedCondition) ? parsedCondition : ToyCondition.Used
                    }).ToList()
                };

                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Could not load profile.";
                Console.WriteLine(ex);
                return View(new PublicProfileViewModel());
            }
        }

    }
}

