using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tutoria_net_core.Models
{
   public interface IAmigoAlmacen
    {
        Amigo dameDatosAmigo(int Id);

        List<Amigo> dateTodosLosAmigos();
        Amigo Nuevo(Amigo amigo);

        Amigo modificar(Amigo modificarAmigo);

        Amigo borrar(int id);
    }

}
