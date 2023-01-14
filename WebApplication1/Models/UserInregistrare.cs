using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class UserInregistrare
    {
        [Required(ErrorMessage = "Adresa de email este obligatorie.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Parola este obligatorie.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Rolul este obligatoriu.")]
        public string Role { get; set; }
    }
}
