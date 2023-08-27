using System.ComponentModel.DataAnnotations;

namespace Ferreteria.Models
{
    public class RegistroViewModel
    {
        [Required(ErrorMessage = "Este campo es requerido")]
        [EmailAddress(ErrorMessage = "Ingrese un correo válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
