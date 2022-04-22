using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog.Extensions.Logging;
using Microsoft.AspNetCore;

namespace tutoria_net_core
{
    public class Program
    {
        // para iniciar la aplicacion,
        //map para mapear las rutas
        //use para ir pasando peticiones 


        public static void Main(string[] args)
        {
           // CreateHostBuilder(args).Build().Run();
            CreateWebHostBuilder(args).Build().Run();
        }

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
                    
        //            webBuilder.UseStartup<Startup>();
        //        });

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) => WebHost.CreateDefaultBuilder(args)
          .ConfigureLogging((hostingContext, logging) =>
          {
              logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
              logging.AddConsole();
              logging.AddDebug();
              logging.AddEventSourceLogger();
              logging.AddNLog();



          }).UseStartup<Startup>();

    }
}
