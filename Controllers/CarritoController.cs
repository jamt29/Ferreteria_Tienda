using Ferreteria.Extensions;
using Ferreteria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Ferreteria.Controllers
{
    public class CarritoController : Controller
    {
        private readonly FerreteriaContext _context;
        public CarritoController(FerreteriaContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult AgregarAlCarrito(int productoId)
        {
            // Se verifica si hay un carrito en la sesión. Si no hay, se crea uno.
            List<int> carrito;
            if (HttpContext.Session.GetObject<List<int>>("CARRITO") == null)
            {
                carrito = new List<int>();
            }
            else
            {
                // Si ya existe un carrito en la sesión, se obtiene.
                carrito = HttpContext.Session.GetObject<List<int>>("CARRITO");
            }

            // Se verifica si el producto ya está en el carrito.
            if (!carrito.Contains(productoId))
            {
                // Si el producto no está en el carrito, se agrega.
                carrito.Add(productoId);
                HttpContext.Session.SetObject("CARRITO", carrito); // Se actualiza el carrito en la sesión.
            }

            // Se redirige al usuario a la página de productos del cliente.
            return RedirectToAction("Productos", "Cliente");
        }


        [HttpPost]
        public IActionResult EliminarDelCarrito(int productoId)
        {
            // Se obtiene el carrito de la sesión.
            List<int> carrito = HttpContext.Session.GetObject<List<int>>("CARRITO");

            // Se verifica si el carrito existe y si el producto está en él.
            if (carrito != null && carrito.Contains(productoId))
            {
                // Se elimina el producto del carrito.
                carrito.Remove(productoId);
                HttpContext.Session.SetObject("CARRITO", carrito); // Se actualiza el carrito en la sesión.
            }

            // Se redirige al usuario a la página del carrito.
            return RedirectToAction("Carrito");
        }


        [AllowAnonymous]
        public IActionResult Carrito()
        {
            // Se obtienen los IDs de productos del carrito de la sesión.
            List<int> carritoIds = HttpContext.Session.GetObject<List<int>>("CARRITO");

            // Lista para almacenar los productos del carrito.
            List<Producto> carritoProductos = new List<Producto>();

            // Si existen IDs en el carrito, se obtienen los productos correspondientes desde la base de datos.
            if (carritoIds != null)
            {
                // Se obtienen los productos cuyos IDs están en el carrito.
                carritoProductos = _context.Productos.Where(p => carritoIds.Contains(p.Id)).ToList();
            }

            // Se muestra la vista de carrito y se pasan los productos del carrito como modelo.
            return View(carritoProductos);
        }


        [Authorize(Roles = "Cliente")]
        public IActionResult RealizarPedido()
        {
            // Obtener los IDs de productos del carrito desde la sesión
            List<int> carrito = HttpContext.Session.GetObject<List<int>>("CARRITO");

            // Si no hay productos en el carrito, redireccionar al carrito para mostrarlo vacío
            if (carrito == null || carrito.Count == 0)
            {
                return RedirectToAction("Carrito");
            }

            // Obtener los productos del carrito desde la base de datos
            List<Producto> productos = _context.Productos.Where(p => carrito.Contains(p.Id)).ToList();

            // Calcular el total de la compra sumando los precios de los productos
            double total = productos.Sum(p => p.Precio);

            // Crear un objeto para el resumen de la compra
            ResumenCompraViewModel resumenCompra = new ResumenCompraViewModel
            {
                Productos = productos,
                Total = total
            };

            // Mostrar la vista de resumen de compra con los productos y el total
            return View(resumenCompra);
        }


        [Authorize(Roles = "Cliente")]
        public IActionResult ConfirmarPedido()
        {
            HttpContext.Session.Remove("CARRITO");
            return View();
        }
    }
}
