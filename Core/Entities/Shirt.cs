
namespace Core.Entities
{
    public class Shirt
    {
        public int Id { get; set; }

        [MaxLength(500)]
        public string Name { get; set; } = String.Empty;
        public string? ImageUrl { get; set; }
        public string Description { get; set; } = null!;
        public double Price { get; set; }

        public int PlayerId { get; set; }
        public Player? Player { get; set; }

        public int SizeId { get; set; }
        public Size? Size { get; set; }

        public int ColorId { get; set; }
        public Color? Color { get; set; }


    }
}
