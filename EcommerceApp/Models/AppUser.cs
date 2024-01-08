using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Required, StringLength(50)]
        public string MobileNo { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int Role { get; set; }
        public bool IsActive { get; set; }
    }
}
