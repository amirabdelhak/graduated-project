using graduated_project.metadatamodels;
using System.ComponentModel.DataAnnotations;

namespace graduated_project.Models
{
    [MetadataType(typeof(Categorymetadata))]
    public class Category
    {
        public int Id { get; set; }
      
        public string? Name { get; set; }
        public virtual List<Product>? Products { get; set; }
    }
}
