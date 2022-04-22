
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace tutoria_net_core.Controllers
{
    public class ErrorController : Controller
    {  // log para errores
        private readonly ILogger<ErrorController> logs;
        public ErrorController(ILogger<ErrorController> log)
        {
            this.logs = log;
        }



        [AllowAnonymous]
        [Route("Error")]
        public IActionResult Error()// que muestre la traza de pila del error
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();


            logs.LogError($"Ruta del ERROR: {exceptionHandlerPathFeature.Path} " +
            $"Excepcion: {exceptionHandlerPathFeature.Error}" +
            $"Traza del ERROR: {exceptionHandlerPathFeature.Error.StackTrace}");

            return View("ErrorGenerico");
        }

        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {

            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "El recurso solicitado no existe";
                    break;
            }

            return View("Error");
        }


    }
}
