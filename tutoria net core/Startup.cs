using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tutoria_net_core.Models;
using tutoria_net_core.ViewModels;

namespace tutoria_net_core
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
            // services.AddRazorPages();

            //------- MODELO VISTA CONTROLADOR ----------
            services.AddDbContext<AppDbContext>();
            // services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ConexionSQL")));
            services.AddMvc();
          /*   services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                options.Filters.Add(new AuthorizeFilter(policy)); //requiera autenticacion a las distintas funcionalidades de la aplicacion

            }).AddXmlSerializerFormatters();*/ // para modelo vista controlador 1 
                                             //  services.AddMvcCore(); // para modelo vista controlador 2 //
                                             //services.AddSingleton< IAmigoAlmacen, MockAmigoRepositorio>();//llamaba al mock donde estan las listas quemadas
            services.AddScoped<IAmigoAlmacen, SQLAmigoRepositorio>();

            /***********onfiguracion para el sistemas de identidades *******************
             *ddErrorDescriber<ErroresCastellano>()  -> sobrecribe las validaciones de ingles y las pone en castellano
             */
            services.AddIdentity<UsuarioAplicacion, IdentityRole>(
                options => {
                   options.SignIn.RequireConfirmedEmail = false;
                   
                }).AddErrorDescriber<ErroresCastellano>().
                AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

        /************* solo ve la informacion las personas logeadas ********/
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Cuentas/Login";
                options.AccessDeniedPath = "/Cuentas/AccesoDenegado";
            });
            
            /*********  CONFIGURAR VALIDACIONES DEL CAMPO PASWORD *******/
            services.Configure<IdentityOptions>(opciones =>
            {
                opciones.Password.RequiredLength = 8;
                opciones.Password.RequiredUniqueChars = 3; //solo pida 3 caracteres
                opciones.Password.RequireNonAlphanumeric = false; // quitar los alfas numericos
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ILogger <Startup> lloger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else if (env.IsProduction() || env.IsStaging())
            {

                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithRedirects("/Error/{0}");
            }

            else
            {
                app.UseExceptionHandler("/Error"); 
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //--------------middleware
            //app.Use(async (context,next) =>
            //{
            //    lloger.LogInformation("XXXXXXX");
            //    await context.Response.WriteAsync("CAMINO 1");
            //    await next();
            //});
            //app.Run(async context =>
            //{
            //    await context.Response.WriteAsync("CAMINO 2");

            //});


            //----------ficheros estaticos------------
            //app.UseHttpsRedirection();
            //DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
            //defaultFilesOptions.DefaultFileNames.Clear();
            //defaultFilesOptions.DefaultFileNames.Add("nodefault.html");
            //app.UseDefaultFiles(defaultFilesOptions);
            //app.UseStaticFiles();

            //-----------exepciones------------
            /*app.Run(async context =>
            {
                // throw new Exception("error fatal");
                await context.Response.WriteAsync("CAMINO 2");

            });*/

            
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseCors();

            //----------- MODELO VISTA CONTROLADOR --------------
            //app.UseCors();
            //app.Run(async context =>
            //{
            //   // throw new Exception("error fatal");
            //    await context.Response.WriteAsync("mvc 2");

            //});
            // fraccion de codigo enruta hacia la clase controller
            //--------- ROUTING ------------

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            /* app.UseEndpoints(endpoints =>
             {
                 endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");// con interogacion no es obligatorio
             });*/
            //------ fin routing ------------------
            //  app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapRazorPages();
            //});
        }
    }
}
