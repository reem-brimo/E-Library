namespace E_Commerce.Core.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; } = 0.0m;

        public string? ImageUrl { get; set; }
    }
}
