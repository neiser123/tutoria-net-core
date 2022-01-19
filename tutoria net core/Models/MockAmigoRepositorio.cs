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
            amigosLista.Add(new Amigo { Id = 1, nombre = "neiser", ciudad = "Cali", email = "neiser@hotmail.com" });
            amigosLista.Add(new Amigo { Id = 2, nombre = "sergio", ciudad = "cucuta", email = "sergio@hotmail.com" });
            amigosLista.Add(new Amigo { Id = 3, nombre = "guillermo", ciudad = "bogota", email = "guille@hotmail.com" });
            amigosLista.Add(new Amigo { Id = 4, nombre = "alex", ciudad = "medellin", email = "alex@hotmail.com" });
             }

        public Amigo dameDatosAmigo(int Id)
        {
            return this.amigosLista.FirstOrDefault(e => e.Id == Id);
        }

        public List<Amigo> dateTodosLosAmigos()
        {
            return amigosLista;
        }
    }
}
