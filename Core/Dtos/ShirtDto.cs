namespace Core.Dtos
{
    public class ShirtDto
    {
        public int? Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public IFormFile? Image { get; set; }

        public string Description { get; set; } = null!;
        public double Price { get; set; }

        public string Player { get; set; } = null!;
        public string Size { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Color { get; set; } = null!;
    }
}
