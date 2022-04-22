using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using tutoria_net_core.Models;

namespace tutoria_net_core.Models
{
    public class Amigo
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Obligatorio"), MaxLength(100, ErrorMessage = "No más de 100 caracteres")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "Obligatorio")]
        [Display(Name = "Email")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Formato incorrecto")]
        public string email { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una ciudad.")]

        public Ciudad? ciudad { get; set; }

        public string rutaFoto { get;set; }
    }
}
