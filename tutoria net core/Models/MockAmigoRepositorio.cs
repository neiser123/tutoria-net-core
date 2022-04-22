using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tutoria_net_core.Models
{
    public class MockAmigoRepositorio :IAmigoAlmacen
    {
        private List<Amigo> amigosLista;

        public MockAmigoRepositorio()
             {
            amigosLista = new List<Amigo>();
            amigosLista.Add(new Amigo { Id = 1, nombre = "neiser", ciudad = Ciudad.Cali, email = "neiser@hotmail.com" });
            amigosLista.Add(new Amigo { Id = 2, nombre = "sergio", ciudad = Ciudad.Buga, email = "sergio@hotmail.com" });
            amigosLista.Add(new Amigo { Id = 3, nombre = "guillermo", ciudad = Ciudad.Buga, email = "guille@hotmail.com" });
            amigosLista.Add(new Amigo { Id = 4, nombre = "alex", ciudad = Ciudad.Medellin, email = "alex@hotmail.com" });
             }

        public Amigo dameDatosAmigo(int Id)
        {
            return this.amigosLista.FirstOrDefault(e => e.Id == Id);
        }

        public List<Amigo> dateTodosLosAmigos()
        {
            return amigosLista;
        }

        public Amigo Nuevo(Amigo amigo)
        {
            amigo.Id = amigosLista.Max(a => a.Id) + 1;
            amigosLista.Add(amigo);
            return amigo;
        }

        public Amigo modificar(Amigo modificarAmigo)
        {
            Amigo amigo = amigosLista.FirstOrDefault(e => e.Id == modificarAmigo.Id);
            if (amigo != null)
            {
                amigo.nombre = modificarAmigo.nombre;
                amigo.email = modificarAmigo.email;
                amigo.ciudad = modificarAmigo.ciudad;
            }
            return amigo;

        }

        public Amigo borrar(int Id)
        {
            Amigo amigo = amigosLista.FirstOrDefault(e => e.Id == Id);
            if (amigo != null)
            {
                amigosLista.Remove(amigo);
            }
            return amigo;

        }
    }
}
