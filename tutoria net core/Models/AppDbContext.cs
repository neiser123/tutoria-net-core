using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tutoria_net_core.ViewModels;

namespace tutoria_net_core.Models
{
    public class AppDbContext : IdentityDbContext<UsuarioAplicacion>
    {
        public AppDbContext() 
        {

        }
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {

        }

        public DbSet<Amigo> Amigos { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                //options.UseSqlServer("Data Source=DESKTOP-68CSEVF;User ID=sa;Password=1234;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
                options.UseSqlServer("server=DESKTOP-68CSEVF;database=GTintegration;trusted_connection=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Amigo>().HasData(new Amigo { Id = 1, nombre = "neiser", ciudad = Ciudad.Cali, email = "neiser@hotmail.com" });
            base.OnModelCreating(modelBuilder);
        }
    }
}
