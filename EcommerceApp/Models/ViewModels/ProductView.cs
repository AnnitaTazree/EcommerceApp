namespace EcommerceApp.Models.ViewModels
{
    public class ProductView
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public double Price { get; set; }
        public double Quantity { get; set; }
        public IFormFile Image { get; set; } = default!;
    }
}
