using graduated_project.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace graduated_project.metadatamodels
{
    public class Ordermetadata
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="this field is required")]
        public DateTime OrderDate { get; set; }
        [Required(ErrorMessage = "this field is required")]
        public DateTime? ShippingDate { get; set; }
        [Required(ErrorMessage = "this field is required")]
        [StringLength(50)]
        public string? ShippingAddress { get; set; }
        [Required(ErrorMessage = "this field is required")]

        public decimal TotalPrice { get; set; }
        [ForeignKey("User")]
        public string? UserId { get; set; }
        public virtual AppUser? User { get; set; }
        public virtual ICollection<ProductOrder>? ProductOrders { get; set; }

    }
}
