using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ferreteria.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

public class FerreteriaContext : IdentityDbContext
{
        public FerreteriaContext (DbContextOptions<FerreteriaContext> options)
            : base(options)
        {
        }

        public DbSet<Ferreteria.Models.Departamento> Departamentos { get; set; }

        public DbSet<Ferreteria.Models.Producto> Productos { get; set; }

        public DbSet<IdentityUser> Users { get; set; }

        public DbSet<IdentityUserRole<string>> UserRoles { get; set; }


 


}
