using EcommerceApp.Models;

namespace EcommerceApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public double Price { get; set; }
        public double Quantity { get; set; }
        public byte[] Image { get; set; } = default!;
        public string ImageFile { get; set; } = default!;
        public List<SalesItem>? SalesItems { get; set; }
    }
}

