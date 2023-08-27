using System.ComponentModel.DataAnnotations;

namespace Ferreteria.Models
{
    public class FerreteriaBase
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        public string Nombre { get; set; }
    }
}
