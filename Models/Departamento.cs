using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ferreteria.Models
{
    public class Departamento: FerreteriaBase
    {
        public string ImgUrl { get; set; }
        [NotMapped]
        public IFormFile ImagenArchivo { get; set; }


    }
}
