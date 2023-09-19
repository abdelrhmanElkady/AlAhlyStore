
namespace Core.Entities
{
    public class Shirt
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = null!;
        public string? ImageUrl { get; set; }
        
        [MaxLength(100)]
        public string Description { get; set; } = null!;
        public double Price { get; set; }
        public string Size { get; set; } = null!;
        public string Color { get; set; } = null!;
        public string Type { get; set; } = null!;

        public int PlayerId { get; set; }
        public Player? Player { get; set; }

       


    }
}
