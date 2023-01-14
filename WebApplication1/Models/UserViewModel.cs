using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "Adresa de email este obligatorie.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Parola este obligatorie.")]
        public string Password { get; set; }
    }
}
