using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tutoria_net_core.Models;

namespace tutoria_net_core.Controllers
{
    public class HomeController : Controller
    {
        private IAmigoAlmacen AmigoAlmacen;
        public HomeController(IAmigoAlmacen amigoAlmacen)
        {
            AmigoAlmacen = amigoAlmacen;
        }

        //public string Index()
        //{
        //    return AmigoAlmacen.dameDatosAmigo(1).email;
        //}

        // devuelve el json  https://localhost:44369/home/details
        //public JsonResult details()
        //{
        //    Amigo modelo = AmigoAlmacen.dameDatosAmigo(1);
        //    return Json(modelo);
        //}

        //----------ruta por medio del controlador que va hacia la carpeta views--------------------
          public ViewResult Index()
          {
              Amigo modelo = AmigoAlmacen.dameDatosAmigo(1);
              return View(modelo);
          }
          public ViewResult details()
          {
              Amigo amigo = AmigoAlmacen.dameDatosAmigo(1);
            //view data
            ViewData["Cabecera"] = "LISTA DE AMIGOS";
            ViewData["Amigo"] = amigo;
            //view bag
            ViewBag.Cabecera = "LISTA DE AMIGOS VIEWBAGS";
            ViewBag.Amigo = amigo;

            return View(amigo);

          }

        //-------------- RUTAS CON URL QUEMADA ---------------
     /*   public ViewResult Index()
        {
            Amigo modelo = AmigoAlmacen.dameDatosAmigo(1);
            return View("~/vistaporurl/Index.cshtml");
        }*/
    }
}
