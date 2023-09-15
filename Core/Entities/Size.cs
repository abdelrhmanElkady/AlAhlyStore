using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Size
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; } = null!;

        public ICollection<Shirt> Shirts { get; set; } = new List<Shirt>();
    }
}
