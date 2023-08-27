using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferreteria
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var politicaUsuariosAutenticados = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser().Build();

            services.AddControllersWithViews(
                    opciones => opciones.Filters.Add(new AuthorizeFilter(politicaUsuariosAutenticados))
                );

            services.AddSession();

            services.AddDbContext<FerreteriaContext>(
            opciones => opciones.UseSqlServer("name=DefaultConecctionString"));

            services.AddAuthentication();

            services.AddIdentity<IdentityUser, IdentityRole>(opciones =>
                 opciones.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<FerreteriaContext>();

            services.PostConfigure<CookieAuthenticationOptions>
                (IdentityConstants.ApplicationScheme, opciones =>
                {
                    opciones.LoginPath = "/usuarios/Login";
                    opciones.AccessDeniedPath = "/usuarios/Login";
                });


            string conString = ConfigurationExtensions.GetConnectionString(

                    this.Configuration, "DefaultConecctionString"

                );

            services.AddDbContext<FerreteriaContext>(
                options => options.UseSqlServer(conString));


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

            });
        }
    }
}
