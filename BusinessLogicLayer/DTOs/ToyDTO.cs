﻿using System;
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
        public string Image { get; set; }
        public string Condition { get; set; }
        public string OwnerId { get; set; }
    }
}
