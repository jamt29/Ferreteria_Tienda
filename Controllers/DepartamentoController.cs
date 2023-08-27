using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ferreteria.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace Ferreteria.Controllers
{
    public class DepartamentoController : Controller
    {
        private readonly FerreteriaContext _context;

        public DepartamentoController(FerreteriaContext context)
        {
            _context = context;
        }

        // GET: Departamento
        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]

        public async Task<IActionResult> Index()
        {
            return View(await _context.Departamentos.ToListAsync());
        }

    

        // GET: Departamento/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departamento = await _context.Departamentos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (departamento == null)
            {
                return NotFound();
            }

            return View(departamento);
        }

        // GET: Departamento/Create
        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]

        public IActionResult Create()
        {
            return View();
        }

        // POST: Departamento/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]

        public async Task<IActionResult> Create([Bind("Id,Nombre")] Departamento departamento, IFormFile ImagenArchivo)
        {
            if (ModelState.IsValid)
            {

                 departamento.ImagenArchivo = ImagenArchivo;

                if (departamento.ImagenArchivo != null && departamento.ImagenArchivo.Length > 0)
                {
                    string wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    string imgFolderpath = Path.Combine(wwwrootPath, "Img");
                    string rutaImagen = Path.Combine(imgFolderpath, ImagenArchivo.FileName);

                    using (var fileStream = new FileStream(rutaImagen, FileMode.Create))
                    {
                        await departamento.ImagenArchivo.CopyToAsync(fileStream);
                    }
                    departamento.ImgUrl = "/Img/" + departamento.ImagenArchivo.FileName;
                }



                _context.Add(departamento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(departamento);
        }

        // GET: Departamento/Edit/5
        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departamento = await _context.Departamentos.FindAsync(id);
            if (departamento == null)
            {
                return NotFound();
            }
            return View(departamento);
        }

        // POST: Departamento/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]

        public async Task<IActionResult> Edit(int id, [Bind("ImgUrl,Id,Nombre")] Departamento departamento, IFormFile ImagenArchivo)
        {
            if (id != departamento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {



                try
                {
                    departamento.ImagenArchivo = ImagenArchivo;

                    if (departamento.ImagenArchivo != null && departamento.ImagenArchivo.Length > 0)
                    {
                        string wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                        string imgFolderpath = Path.Combine(wwwrootPath, "Img");
                        string rutaImagen = Path.Combine(imgFolderpath, ImagenArchivo.FileName);

                        using (var fileStream = new FileStream(rutaImagen, FileMode.Create))
                        {
                            await departamento.ImagenArchivo.CopyToAsync(fileStream);
                        }
                        departamento.ImgUrl = "/Img/" + departamento.ImagenArchivo.FileName;
                    }
                    _context.Update(departamento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartamentoExists(departamento.Id))
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
            return View(departamento);
        }

        // GET: Departamento/Delete/5
        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departamento = await _context.Departamentos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (departamento == null)
            {
                return NotFound();
            }

            return View(departamento);
        }

        // POST: Departamento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var departamento = await _context.Departamentos.FindAsync(id);
            _context.Departamentos.Remove(departamento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartamentoExists(int id)
        {
            return _context.Departamentos.Any(e => e.Id == id);
        }
    }
}
