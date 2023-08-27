using Ferreteria.Migrations;
using Ferreteria.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferreteria.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> roleManager;
        private readonly FerreteriaContext _context;


        public UsuariosController(Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> roleManager, FerreteriaContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            _context = context;

        }

        [AllowAnonymous]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Registro(RegistroViewModel modelo)
        {


            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var usuario = new IdentityUser()
            {
                Email = modelo.Email,
                UserName = modelo.Email,
            };

            await roleManager.CreateAsync(new IdentityRole("Admin"));
            await roleManager.CreateAsync(new IdentityRole("Cliente"));


            var resultado = await userManager.CreateAsync(usuario,
                password: modelo.Password);

            var usuariosRegistrados = await userManager.Users.ToListAsync();

            if (usuariosRegistrados.Count == 1)
            {
                // Si es el primer usuario registrado, asignar el rol de admin
                await userManager.AddToRoleAsync(usuario, "admin");
            }
            else
            {
                // Si no es el primer usuario registrado, asignar el rol de cliente
                await userManager.AddToRoleAsync(usuario, "cliente");
            }


            if (resultado.Succeeded)
            {
                var usuarios = await userManager.FindByEmailAsync(modelo.Email);
                var roles = await userManager.GetRolesAsync(usuarios);

                if (roles.Contains("Admin"))
                {
                    await signInManager.SignInAsync(usuario, isPersistent: false);
                    return RedirectToAction("IndexAdmin", "Home");

                }
                else
                {
                    await signInManager.SignInAsync(usuario, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(modelo);
            }

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var resultado = await signInManager.PasswordSignInAsync(
                    modelo.Email, modelo.Password, modelo.Recuerdame,
                    lockoutOnFailure: false
                );

            if (resultado.Succeeded)
            {
                var usuario = await userManager.FindByEmailAsync(modelo.Email);
                var roles = await userManager.GetRolesAsync(usuario);

                if (roles.Contains("Admin"))
                {
                    return RedirectToAction("IndexAdmin", "Home");

                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }

            }
            else
            {
                ModelState.AddModelError(string.Empty,
                    "El nombre de usuario o la contraseña son incorrectos");
                return View(modelo);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public async Task<IActionResult> Usuarios(string? roleId)
        {
            // Obtener una lista de usuarios para mostrar en la vista
            var usuarios = _context.Users.ToList();

            var rolesList = roleManager.Roles.ToList();
            ViewBag.rolesList = rolesList;

            // Pasar la lista de usuarios a la vista
            return View(usuarios);
        }

        [HttpPost]
        public IActionResult CambiarRol(string UserId, string RoleId)
        {
            // Obtener el usuario y el rol
            var user = userManager.FindByIdAsync(UserId).Result;
            var role = roleManager.FindByIdAsync(RoleId).Result;

            if (user != null && role != null)
            {
                // Remover todos los roles actuales del usuario
                var removeResult = userManager.RemoveFromRolesAsync(user, userManager.GetRolesAsync(user).Result).Result;

                if (removeResult.Succeeded)
                {
                    // Asignar el nuevo rol al usuario
                    var addResult = userManager.AddToRoleAsync(user, role.Name).Result;

                    if (addResult.Succeeded)
                    {
                        // El cambio de rol fue exitoso
                        return RedirectToAction("Usuarios");
                    }
                }
            }

            // Si ocurre un error durante el cambio de rol, retornar un error o redireccionar a una página de error
            return RedirectToAction("Error");
        }


        
    }
}
