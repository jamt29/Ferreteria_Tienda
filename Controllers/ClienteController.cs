using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Ferreteria.Controllers
{
    public class ClienteController : Controller
    {
        private readonly FerreteriaContext _context;


        public ClienteController(FerreteriaContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Productos()
        {
            return View(await _context.Productos.ToListAsync());
        }

        [AllowAnonymous]
        public async Task<IActionResult> DetalleProductoAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Departamento)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }
        
        [AllowAnonymous]
        public async Task<IActionResult> Departamentos()
        {
            return View(await _context.Departamentos.ToListAsync());
        }

        [AllowAnonymous]
        public async Task<IActionResult> DetalleDepartamento(int? id)
        {
            var productosDepartamento = await _context.Productos.Where(p => p.DepartamentoId == id).ToListAsync();
            return View(productosDepartamento);
        }
    }
}
