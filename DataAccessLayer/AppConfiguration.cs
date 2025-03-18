using Microsoft.Extensions.Configuration;

namespace DataAccessLayer
{
    public class AppConfiguration
    {
        private readonly IConfiguration _configuration;

        public AppConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnectionString()
        {
            return _configuration.GetConnectionString("MySqlConnection");
        }
    }
}
