using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Ferreteria.Models
{
    public class LoginViewModel : RegistroViewModel
    {
        [Display(Name = "Recuerdame")]
        public bool Recuerdame { get; set; }
    }
}
