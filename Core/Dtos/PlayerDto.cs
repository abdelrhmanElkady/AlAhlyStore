
namespace Core.Dtos
{
    public class PlayerDto
    {
        public int? Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } =  null!;
        public string? ImageUrl { get; set; }
        public IFormFile? Image { get; set; }
    }
}
