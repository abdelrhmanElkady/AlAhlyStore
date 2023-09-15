﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos
{
    public class ShirtDto
    {
        public int Id { get; set; }

        [MaxLength(500)]
        public string Name { get; set; } = String.Empty;
        public string? ImageUrl { get; set; }
        public string Description { get; set; } = null!;
        public double Price { get; set; }

        public string? Player { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
    }
}