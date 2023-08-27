using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ferreteria.Models;
using Microsoft.Extensions.Hosting;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace Ferreteria.Controllers
{
    public class ProductoController : Controller
    {
        private readonly FerreteriaContext _context;
        private readonly IWebHostEnvironment _hostEnvironment; 


        public ProductoController(FerreteriaContext context)
        {
            _context = context;
        }

        // GET: Producto
        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]

        public async Task<IActionResult> Index() 
        {
            var ferreteriaContext = _context.Productos.Include(p => p.Departamento);
            return View(await ferreteriaContext.ToListAsync());
        }

        

        // GET: Producto/Details/5
        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]

        public async Task<IActionResult> Details(int? id)
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


        // GET: Producto/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["DepartamentoId"] = new SelectList(_context.Departamentos, "Id", "Nombre");
            return View();
        }

        // POST: Producto/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("ImgUrl,Precio,Descripcion,DepartamentoId,Id,Nombre")] Producto producto, IFormFile ImagenArchivo)
        {
            if (ModelState.IsValid)
            {
                producto.ImagenArchivo = ImagenArchivo;

                if (producto.ImagenArchivo != null && producto.ImagenArchivo.Length > 0)
                {
                    string wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    string imgFolderpath = Path.Combine(wwwrootPath, "Img");
                    string rutaImagen = Path.Combine(imgFolderpath, ImagenArchivo.FileName);

                    using (var fileStream = new FileStream(rutaImagen, FileMode.Create))
                    {
                        await producto.ImagenArchivo.CopyToAsync(fileStream);
                    }
                    producto.ImgUrl = "/Img/" + producto.ImagenArchivo.FileName;
                }

                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            ViewData["DepartamentoId"] = new SelectList(_context.Departamentos, "Id", "Nombre", producto.DepartamentoId);
            return View(producto);
        }

        // GET: Producto/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            ViewData["DepartamentoId"] = new SelectList(_context.Departamentos, "Id", "Nombre", producto.DepartamentoId);
            return View(producto);
        }

        // POST: Producto/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("ImgUrl, Precio,Descripcion,DepartamentoId,Id,Nombre")] Producto producto)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartamentoId"] = new SelectList(_context.Departamentos, "Id", "Nombre", producto.DepartamentoId);
            return View(producto);
        }

        // GET: Producto/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Producto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }
    }
}
