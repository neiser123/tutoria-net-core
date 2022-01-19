using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tutoria_net_core.Models;

namespace tutoria_net_core.ViewModels
{
    public class DetallesView
    {
        public  string Titulo { get; set; }
        public  string Subtitulo { get; set; }
        public  Amigo amigo { get; set; }
    }
}
