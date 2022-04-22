using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tutoria_net_core.Utilidades
{
    public class ValidarNombreUsuario: ValidationAttribute
    {
        private readonly string usuario;
        public ValidarNombreUsuario(string usuario)
        {
            this.usuario = usuario;
        }

        public override bool IsValid(object value)
        {
            Boolean permitido = true;
            if (value.ToString().Contains("joder"))
                permitido = false;

            return permitido;
           
        }
    }
}
