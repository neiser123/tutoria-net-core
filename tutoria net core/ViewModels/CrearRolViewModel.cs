using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tutoria_net_core.ViewModels
{
    public class CrearRolViewModel
    {
        [Required(ErrorMessage ="Este campo es obligatorio")]
        [Display(Name = "Rol")]
        public string NombreRol { get; set; }
    }
}
