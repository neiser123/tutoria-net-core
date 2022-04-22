using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using tutoria_net_core.Models;
using tutoria_net_core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Hosting;

namespace tutoria_net_core.Controllers
{
    [Authorize] // esta linea limita a que solo los usuarios logeados puedas hacer el crud  en este controlador
    // [AllowAnonymous] //PERMITIR A TODO EL MUNDO
    public class HomeController : Controller
    {
        private IAmigoAlmacen AmigoAlmacen;
        private IHostEnvironment hosting;
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
        [Route("")]
        [Route("Home")]
        [Route("Home/Index")]
       // [AllowAnonymous] //PERMITIR A TODO EL MUNDO
        public ViewResult Index(int id)
          {
            //  Amigo modelo = AmigoAlmacen.dameDatosAmigo(1);// un solo dato de la lista
           
            var modelo1 = AmigoAlmacen.dateTodosLosAmigos();
              return View(modelo1);
          }

        [Route("Home/details/{id?}")]// al poner el signo de interogacion no deja que sea obligatorio
        public ViewResult details(int? id)
          {
          //  throw new Exception("FORZANDO EL ERROR.........."); //PARA FORZAR VER LAS EXCEPCIONES .

            //view data
            Amigo amigo = AmigoAlmacen.dameDatosAmigo(id ?? 1); //si viene nulo enviamos 1 por defecto
             ViewData["Cabecera"] = "LISTA DE AMIGOS";
             ViewData["Amigo"] = amigo;
             //view bag
             ViewBag.Cabecera = "LISTA DE AMIGOS VIEWBAGS";
             ViewBag.Amigo = amigo;


            return View(amigo);

          }
        [Route("Home/create")]
        [HttpGet]
        public ViewResult create()
        {

            return View();
        }

        [Route("Home/create")]
        [HttpPost]
        public IActionResult create(CrearAmigoModelo a)
        {

            if (ModelState.IsValid)
            {
                string guidImagen = null;
                if (a.Foto != null)
                {
                    string nn = Directory.GetCurrentDirectory();
                    //string nn1 = hosting.ContentRootPath;
                    //if (string.IsNullOrWhiteSpace(hosting.WebRootPath))
                    //{
                     //  hosting.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    //}
                    string ficherosImagenes = Path.Combine(Directory.GetCurrentDirectory()+ "/wwwroot/images/", "");//obtenemos ruta de las imagenes en el satelite
                    guidImagen = Guid.NewGuid().ToString() + a.Foto.FileName;//
                    string rutaDefinitiva = Path.Combine(ficherosImagenes + guidImagen);
                    a.Foto.CopyTo(new FileStream(rutaDefinitiva, FileMode.Create));
                }
                Amigo amigo = new Amigo();
                amigo.nombre = a.nombre;
                amigo.email = a.email;
                amigo.ciudad = a.ciudad;
                amigo.rutaFoto = guidImagen;
                AmigoAlmacen.Nuevo(amigo);
            return RedirectToAction("details", new { id = amigo.Id });
            }
            return View();
        }
        public ViewResult DetalleViewModel()
        {
          
            DetallesView detalle = new DetallesView();
            detalle.amigo = AmigoAlmacen.dameDatosAmigo(1);
            detalle.Titulo = "LISTA DE AMIGOS VIEW MODEL";
            detalle.Subtitulo = "mis amix queridos jajajajajaj";

            return View(detalle);

        }

        //metodo para mostrar los datos en la vista al momento de darle editar
        [Route("Home/Edit")]
        [HttpGet]
        public ViewResult Edit(int id)
        {
            Amigo amigo = AmigoAlmacen.dameDatosAmigo(id);
            EditarAmigoModelo amigoEditar = new EditarAmigoModelo
            {
                Id = amigo.Id,
                nombre = amigo.nombre,
                email = amigo.email,
                ciudad = amigo.ciudad,
                rutaFotoExistente = amigo.rutaFoto
            };
            return View(amigoEditar);
        }

        //metodo que edita los datos
        [Route("Home/Edit")]
        [HttpPost]
        public IActionResult Edit(EditarAmigoModelo model)
        {
            //Comprobamos que los datos son correctos
            if (ModelState.IsValid)
            {
                // Obtenemos los datos de nuestro amigo de la BBDD
                Amigo amigo = AmigoAlmacen.dameDatosAmigo(model.Id);
                // Actualizamos los datos de nuestro objeto del modelo
                amigo.nombre = model.nombre;
                amigo.email = model.email;
                amigo.ciudad = model.ciudad;


                if (model.Foto != null)
                {
                    //Si el usuario sube una foto.Debe borrarse la anterior
                    if (model.rutaFotoExistente != null)
                    {
                       // string ruta = Path.Combine(hosting.WebRootPath, "images", model.rutaFotoExistente);
                        string ruta = Path.Combine(Directory.GetCurrentDirectory() + "/wwwroot/images/"+ model.rutaFotoExistente, "");//obtenemos ruta de las imagenes en el satelite
                        System.IO.File.Delete(ruta);//borramos la imagen en la carpeta imagenes del proyecto
                    }
                    //Guardamos la foto en wwwroot/images
                    amigo.rutaFoto = SubirImagen(model);
                }

                Amigo amigoModificado = AmigoAlmacen.modificar(amigo);

                return RedirectToAction("index");
            }

            return View(model);
        }


        private string SubirImagen(EditarAmigoModelo model)
        {
            string nombreFichero = null;

            if (model.Foto != null)
            {
                string carpetaSubida = Path.Combine(Directory.GetCurrentDirectory() + "/wwwroot/images/", "");//obtenemos ruta de las imagenes en el satelite
                //string carpetaSubida = Path.Combine(hosting.WebRootPath, "images");
                nombreFichero = Guid.NewGuid().ToString() + "_" + model.Foto.FileName;
                string ruta = Path.Combine(carpetaSubida, nombreFichero);
                using (var fileStream = new FileStream(ruta, FileMode.Create))
                {
                    model.Foto.CopyTo(fileStream);
                }
            }

            return nombreFichero;
        }

        //-------------- RUTAS CON URL QUEMADA ---------------
        /*   public ViewResult Index()
           {
               Amigo modelo = AmigoAlmacen.dameDatosAmigo(1);
               return View("~/vistaporurl/Index.cshtml");
           }*/
    }
}
