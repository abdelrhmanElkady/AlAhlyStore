using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos
{
    public class PlayerDto
    {
        public int Id { get; set; }

        [MaxLength(500)]
        public string Name { get; set; } = String.Empty;
        public string? ImageUrl { get; set; }
    }
}
