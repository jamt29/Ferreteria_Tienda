using Ferreteria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Ferreteria.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public IEnumerable<Departamento> listaDepartamentos { get; set; }
        private readonly FerreteriaContext _context;

        public HomeController(ILogger<HomeController> logger, FerreteriaContext context)
        {
            _logger = logger;
            _context = context;
        }

        [AllowAnonymous]

        public IActionResult Index()
        {
            ViewBag.departamentos = _context.Departamentos;

            var randomProducts = _context.Productos
            .OrderBy(x => Guid.NewGuid()) // Ordena los productos de forma aleatoria
            .Take(4) // Obtén los primeros 4 productos aleatorios
            .ToList();

            ViewBag.productos = randomProducts;

            return View();
        }

        [Authorize(Roles = "Admin")]

        public IActionResult IndexAdmin()
        {
           
          

            return View();
        }

        public IEnumerable<Departamento> GetDepartamentos()
        {
            return _context.Departamentos;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
