using EcommerceApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApp.Models
{
    public class SalesItem
    {
        public int Id { get; set; }
        [ForeignKey("SalesOrder")]
        public int SalesOrderId { get; set; }
        public SalesOrder? SalesOrder { get; set; }
        public double UnitPrice { get; set; }
        public double Quantity { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product? Product { get; set; }

    }
}
