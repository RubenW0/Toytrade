    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace BusinessLogicLayer.DTOs
    {
        public class ToyDTO
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Condition { get; set; }
            public int UserId { get; set; }
            public string Username { get; set; }

            public string ImagePath { get; set; } 
            public IFormFile ImageFile { get; set; }

            public string Image => ImagePath;
        }
    }
