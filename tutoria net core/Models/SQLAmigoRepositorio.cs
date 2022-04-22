using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tutoria_net_core.Models
{
    public class SQLAmigoRepositorio : IAmigoAlmacen
    {
        private readonly AppDbContext contexto;
        private List<Amigo> amigosLista;

        public SQLAmigoRepositorio(AppDbContext contexto)
        {
            this.contexto = contexto;
        }
        public Amigo borrar(int id)
        {
            Amigo amigo = contexto.Amigos.Find(id);
            if (amigo != null)
            {
                contexto.Amigos.Remove(amigo);
                contexto.SaveChanges();
            }
            return amigo;
        }

        public Amigo dameDatosAmigo(int Id)
        {
            return contexto.Amigos.Find(Id);

        }

        public List<Amigo> dateTodosLosAmigos()
        {
            amigosLista = contexto.Amigos.ToList<Amigo>();
            return amigosLista;
        }

        public Amigo modificar(Amigo modificarAmigo)
        {
            var employee = contexto.Amigos.Attach(modificarAmigo);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            contexto.SaveChanges();
            return modificarAmigo;
        }

        public Amigo Nuevo(Amigo amigo)
        {
            contexto.Amigos.Add(amigo);
            contexto.SaveChanges();
            return amigo;
        }
    }
}
