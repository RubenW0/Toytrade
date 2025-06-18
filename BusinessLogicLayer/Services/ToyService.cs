using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.IRepositories;
using Microsoft.Extensions.Hosting; 
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace BusinessLogicLayer.Services
{
    public class ToyService
    {
        private readonly IToyRepository _toyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHostEnvironment? _env;
        private readonly ILogger<ToyService> _logger;

        public ToyService(IToyRepository toyRepository, IUserRepository userRepository, ILogger<ToyService> logger, IHostEnvironment? env = null)
        {
            _toyRepository = toyRepository;
            _userRepository = userRepository;
            _env = env;
            _logger = logger;
        }

        private void LogErrorWithMethodName(Exception ex, string? extraMessage = null, [CallerMemberName] string callerName = "")
        {
            var msg = $"Exception in {callerName}";
            if (!string.IsNullOrEmpty(extraMessage))
                msg += $": {extraMessage}";

            _logger.LogError(ex, msg);
        }

        public List<ToyDTO> GetAllToys()
        {
            try
            {
                var toys = _toyRepository.GetAllToys();
                foreach (var toy in toys)
                {
                    toy.Username = _userRepository.GetUsernameById(toy.UserId);
                }
                return toys;
            }
            catch (Exception ex)
            {
                LogErrorWithMethodName(ex, "Error while retrieving all toys in service.");
                throw;
            }
        }

        public List<ToyDTO> GetToysByUserId(int userId)
        {
            try
            {
                var toys = _toyRepository.GetToysByUserId(userId);
                foreach (var toy in toys)
                {
                    toy.Username = _userRepository.GetUsernameById(toy.UserId);
                }
                return toys;
            }
            catch (Exception ex)
            {
                LogErrorWithMethodName(ex, $"Error while retrieving toys for user {userId} in service.");
                throw;
            }
        }

        public ToyDTO GetToyById(int toyId)
        {
            return _toyRepository.GetToyById(toyId);
        }

        public void AddToy(ToyDTO toy)
        {
            try
            {
                if (toy.ImageFile != null && toy.ImageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_env.ContentRootPath, "wwwroot", "images");
                    Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid() + Path.GetExtension(toy.ImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using var stream = new FileStream(filePath, FileMode.Create);
                    toy.ImageFile.CopyTo(stream);

                    toy.ImagePath = "/images/" + uniqueFileName;
                }

                _toyRepository.AddToy(toy);
            }
            catch (Exception ex)
            {
                LogErrorWithMethodName(ex, "Error while adding toy in service.");
                throw;
            }
        }

        public void UpdateToy(ToyDTO toy)
        {
            try
            {
                if (toy.ImageFile != null && toy.ImageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_env.ContentRootPath, "wwwroot", "images");
                    Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid() + Path.GetExtension(toy.ImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using var stream = new FileStream(filePath, FileMode.Create);
                    toy.ImageFile.CopyTo(stream);

                    toy.ImagePath = "/images/" + uniqueFileName;
                }

                _toyRepository.UpdateToy(toy);
            }
            catch (Exception ex)
            {
                LogErrorWithMethodName(ex, $"Error while updating toy with ID {toy.Id} in service.");
                throw;
            }
        }

        public void DeleteToy(int toyId)
        {
            try
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
            catch (Exception ex)
            {
                LogErrorWithMethodName(ex, $"Error while deleting toy with ID {toyId} in service.");
                throw;
            }
        }
    }
}
