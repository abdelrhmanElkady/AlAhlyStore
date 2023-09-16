
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
        public string Size { get; set; } = String.Empty;
        public string Color { get; set; } = String.Empty;

        public int PlayerId { get; set; }
        public Player? Player { get; set; }

       


    }
}
