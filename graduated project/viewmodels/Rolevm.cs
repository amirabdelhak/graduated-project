using System.ComponentModel.DataAnnotations;

namespace graduated_project.viewmodels
{
    public class Rolevm
    {
        [Required]
        [StringLength(20)]
        public string? name { get; set; }
    }
}
