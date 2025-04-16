using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.IRepositorys;
using Microsoft.Extensions.Hosting; 
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Services
{
    public class ToyService
    {
        private readonly IToyRepository _toyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHostEnvironment _env;


        public ToyService(IToyRepository toyRepository, IUserRepository userRepository, IHostEnvironment env)
        {
            _toyRepository = toyRepository;
            _userRepository = userRepository;
            _env = env;
        }

        public List<ToyDTO> GetAllToys()
        {
            var toys = _toyRepository.GetAllToys();

            foreach (var toy in toys)
            {
                toy.Username = _userRepository.GetUsernameById(toy.UserId);
            }

            return toys;
        }

        public List<ToyDTO> GetToysByUserId(int userId)
        {
            var toys = _toyRepository.GetToysByUserId(userId);

            foreach (var toy in toys)
            {
                toy.Username = _userRepository.GetUsernameById(toy.UserId);
            }

            return toys;
        }

        public void AddToy(ToyDTO toy)
        {
            if (toy.ImageFile != null && toy.ImageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_env.ContentRootPath, "wwwroot", "images");
                Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid() + Path.GetExtension(toy.ImageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    toy.ImageFile.CopyTo(stream);
                }

                toy.ImagePath = "/images/" + uniqueFileName;
            }

            _toyRepository.AddToy(toy);
        }


        public void UpdateToy(ToyDTO toy)
        {
            if (toy.ImageFile != null && toy.ImageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_env.ContentRootPath, "wwwroot", "images");
                Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid() + Path.GetExtension(toy.ImageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    toy.ImageFile.CopyTo(stream);
                }

                toy.ImagePath = "/images/" + uniqueFileName;
            }

            _toyRepository.UpdateToy(toy);
        }


        public void DeleteToy(int toyId)
        {
            var toy = _toyRepository.GetAllToys().FirstOrDefault(t => t.Id == toyId);
            if (toy != null && !string.IsNullOrEmpty(toy.ImagePath))
            {
                string fullPath = Path.Combine(_env.ContentRootPath, "wwwroot", toy.ImagePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }

            _toyRepository.DeleteToy(toyId);
        }


    }
}
