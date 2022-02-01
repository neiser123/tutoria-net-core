using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tutoria_net_core.Models;

namespace tutoria_net_core.Models
{
    public class Amigo
    {
        public int Id { get; set; }
        public string nombre { get; set; }
        public string email { get; set; }
        public Ciudad ciudad { get; set; }
    }
}
