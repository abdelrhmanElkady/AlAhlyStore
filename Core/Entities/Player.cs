using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Player
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public ICollection<Shirt> Shirts { get; set; } = new List<Shirt>();
    }
}
