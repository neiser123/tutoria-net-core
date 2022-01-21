using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tutoria_net_core.Models;

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
            services.AddMvc(); // para modelo vista controlador 1 
           //  services.AddMvcCore(); // para modelo vista controlador 2 //
            services.AddSingleton< IAmigoAlmacen, MockAmigoRepositorio>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ILogger <Startup> lloger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
            app.UseRouting();
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
