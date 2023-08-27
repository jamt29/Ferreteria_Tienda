using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ferreteria.Models
{
    public class Producto: FerreteriaBase
    {
        public string ImgUrl { get; set; }
        [NotMapped]
        public IFormFile ImagenArchivo { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        public double Precio { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        public string Descripcion { get; set; }

        public int DepartamentoId { get; set; }

        public Departamento Departamento { get; set; }


    }
}
