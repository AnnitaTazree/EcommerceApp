using EcommerceApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApp.Models
{
    public class SalesOrder
    {
        public int Id { get; set; }
        public string OrderNo { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public List<SalesItem>? SalesItems { get; set; }
    }
}

