using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tutoria_net_core.ViewModels
{
    public class EditarRolViewModel
    {

        public string Id { get; set; }
        public EditarRolViewModel()
        {
            Usuarios = new List<string>();
        }


        [Required(ErrorMessage = "El nombre del rol es obligatorio")]
        public string RolNombre { get; set; }

        public List<string> Usuarios { get; set; }
    }
}
