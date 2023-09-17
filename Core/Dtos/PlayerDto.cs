
namespace Core.Dtos
{
    public class PlayerDto
    {
        public int? Id { get; set; }

        [MaxLength(500)]
        public string Name { get; set; } = String.Empty;
        public string? ImageUrl { get; set; }
        public IFormFile? Image { get; set; }
    }
}
