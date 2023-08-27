using System;

namespace Ferreteria.Models
{
    public class Carrito
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; }
        public double Precio { get; set; }

        public Producto Producto { get; set; }
    }
}
