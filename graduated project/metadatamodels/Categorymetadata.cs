using graduated_project.Models;
using System.ComponentModel.DataAnnotations;

namespace graduated_project.metadatamodels
{
    public class Categorymetadata
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="this field is required")]
        [StringLength(20)]
        public string? Name { get; set; }
        public virtual List<Product>? Products { get; set; }
    }
}
