using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tutoria_net_core.ViewModels
{
    public class UsuarioAplicacion:IdentityUser
    {
        public string ayudaPass { get; set; }
    }
}
