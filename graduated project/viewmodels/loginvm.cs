using System.ComponentModel.DataAnnotations;
namespace graduated_project.viewmodels
{
    public class loginvm
    {
        [Required(ErrorMessage = "this field is required")]
        public string? username { get; set; }
        [Required(ErrorMessage = "this field is required")]
        public string? password { get; set; }

    }

}