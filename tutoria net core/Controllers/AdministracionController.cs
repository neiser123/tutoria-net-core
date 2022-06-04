using tutoria_net_core.Models;
using tutoria_net_core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ejemplo1.Controllers
{
    [Authorize(Roles = "Administrador,Dios")]//establece que solo los usuarios asignados a estos roles pueden ver las pantallas que hay en este controller

    public class AdministracionController : Controller
    {
        private readonly RoleManager<IdentityRole> gestionRoles;
        private readonly UserManager<UsuarioAplicacion> gestionUsuarios;
        private readonly ILogger<AdministracionController> log;


        public AdministracionController(RoleManager<IdentityRole> gestionRoles,
                                        UserManager<UsuarioAplicacion> gestionUsuarios,
                                       ILogger<AdministracionController> log)
        {
            this.gestionRoles = gestionRoles;
            this.gestionUsuarios = gestionUsuarios;
            this.log = log;
        }

        [HttpGet]
        [Route("Administracion/CrearRol")]
        [AllowAnonymous]
        public IActionResult CrearRol()
        {
            return View();
        }

        [HttpPost]
        [Route("Administracion/CrearRol")]
        [AllowAnonymous]
        public async Task<IActionResult> CrearRol(CrearRolViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.NombreRol
                };

                IdentityResult result = await gestionRoles.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("index", "home");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

       [HttpGet]
        [Route("Administracion/Roles")]
        [AllowAnonymous]
        public IActionResult ListaRoles()
        {
            var roles = gestionRoles.Roles;
            return View(roles);
        }
        
        [HttpGet]
        [Route("Administracion/EditarRol")]
        [AllowAnonymous]
        public async Task<IActionResult> EditarRol(string id)
                {
                    //Buscamos el rol por el id
                    var rol = await gestionRoles.FindByIdAsync(id);

                    if (rol == null)
                    {
                        ViewBag.ErrorMessage = $"Rol con el Id = {id} no fue encontrado";
                        return View("Error");
                    }
                    //obtenemos todos los usuarios para ese rol
                    var model = new EditarRolViewModel
                    {
                        Id = rol.Id,
                        RolNombre = rol.Name
                    };

            // Obtenemos todos los usuarios
                   var userList = gestionUsuarios.Users.ToList();
                    foreach (var usuario in userList)
                    {
                        if (await gestionUsuarios.IsInRoleAsync(usuario, rol.Name))
                        {

                            model.Usuarios.Add(usuario.UserName);
                        }
                    }

                    return View(model);
                }

      

                [HttpPost]
                [Route("Administracion/EditarRol")]
        //[Authorize(Policy = "EditarRolPolicy")]
        [AllowAnonymous]
        public async Task<IActionResult> EditarRol(EditarRolViewModel model)
                {
                    var rol = await gestionRoles.FindByIdAsync(model.Id);

                    if (rol == null)
                    {
                        ViewBag.ErrorMessage = $"Rol con el Id = {model.Id} no fue encontrado";
                        return View("Error");
                    }
                    else
                    {// si existe pasamos el nombre del rol al objeto que vamos amodidicar
                        rol.Name = model.RolNombre;

                        var resultado = await gestionRoles.UpdateAsync(rol);

                        if (resultado.Succeeded)
                        {
                            return RedirectToAction("ListaRoles");
                        }

                        foreach (var error in resultado.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }

                        return View(model);
                    }
                }

                [HttpPost]
                [Route("Administracion/BorrarRol")]
                [Authorize(Policy = "BorrarRolPolicy")]
        [AllowAnonymous]
        public async Task<IActionResult> BorrarRol(string id)
                {
                    var rol = await gestionRoles.FindByIdAsync(id);

                    if (rol == null)
                    {
                        ViewBag.ErrorMessage = $"Rol con el Id = {id} no fue encontrado";
                        return View("Error");
                    }
                    else
                    {
                        try
                        {
                            var result = await gestionRoles.DeleteAsync(rol);

                            if (result.Succeeded)
                            {
                                return RedirectToAction("ListaRoles");
                            }

                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }

                            return View("ListaRoles");
                        }
                        // Si da este tipo de excepcion sabemos que no podemos elimiar el rol porque tiene usuarios
                        catch (DbUpdateException ex)
                        {
                            log.LogError($"Se produjo un error al borrar el rol: {ex}");
                            //Pasamos el titulo del error y el mensage
                            ViewBag.ErrorTitle = $"El rol {rol.Name} está siendo utilizado";
                            ViewBag.ErrorMessage = $"El rol {rol.Name}  no puede ser borrado porque contiene usuarios.Antes de borrar el rol quita los usuarios de dicho rol.";
                            return View("ErrorGenerico");
                        }

                    }
                }
     

              [HttpGet]
              [Route("Administracion/EditarUsuarioRol")]
        [AllowAnonymous]
        public async Task<IActionResult> EditarUsuarioRol(string rolId)
              {
                  ViewBag.roleId = rolId;

                  var role = await gestionRoles.FindByIdAsync(rolId);

                  if (role == null)
                  {
                      ViewBag.ErrorMessage = $"Rol con el Id = {rolId} no fue encontrado";
                      return View("Error");
                  }

                  var model = new List<UsuarioRolModelo>();
            // Obtenemos todos los usuarios
            var userList = gestionUsuarios.Users.ToList();

            foreach (var user in userList)
                  {
                      var usuarioRolModelo = new UsuarioRolModelo
                      {
                          UsuarioId = user.Id,
                          UsuarioNombre = user.UserName
                      };

                      if (await gestionUsuarios.IsInRoleAsync(user, role.Name))
                      {
                          usuarioRolModelo.EstaSeleccionado = true;
                      }
                      else
                      {
                          usuarioRolModelo.EstaSeleccionado = false;
                      }

                      model.Add(usuarioRolModelo);
                  }

                  return View(model);
              }

              [HttpPost]
              [Route("Administracion/EditarUsuarioRol")]
        [AllowAnonymous]
        public async Task<IActionResult> EditarUsuarioRol(List<UsuarioRolModelo> model,
                                              string rolId)
              {
                  var rol = await gestionRoles.FindByIdAsync(rolId);

                  if (rol == null)
                  {
                      ViewBag.ErrorMessage = $"Rol con el Id = {rolId} no fue encontrado";
                      return View("Error");
                  }

                  for (int i = 0; i < model.Count; i++)
                  {
                      var user = await gestionUsuarios.FindByIdAsync(model[i].UsuarioId);

                      IdentityResult result = null;

                      if (model[i].EstaSeleccionado && !(await gestionUsuarios.IsInRoleAsync(user, rol.Name)))//los selecionados los agregamos a base de datos
                      {
                          result = await gestionUsuarios.AddToRoleAsync(user, rol.Name);
                      }
                      else if (!model[i].EstaSeleccionado && await gestionUsuarios.IsInRoleAsync(user, rol.Name))// los no selecionados los borramos
                      {
                          result = await gestionUsuarios.RemoveFromRoleAsync(user, rol.Name);
                      }
                      else
                      {
                          continue;
                      }

                      if (result.Succeeded)
                      {
                          if (i < (model.Count - 1))
                              continue;
                          else
                              return RedirectToAction("EditarRol", new { Id = rolId });
                      }
                  }

                  return RedirectToAction("EditarRol", new { Id = rolId });
              }
        /*

              [HttpGet]
              [Route("Administracion/ListaUsuarios")]
              public IActionResult ListaUsuarios()
              {
                  var usuarios = gestionUsuarios.Users;
                  return View(usuarios);
              }

              [HttpGet]
              [Route("Administracion/EditarUsuario")]
              public async Task<IActionResult> EditarUsuario(string id)
              {
                  var usuario = await gestionUsuarios.FindByIdAsync(id);

                  if (usuario == null)
                  {
                      ViewBag.ErrorMessage = $"Usuario con Id = {id} no fue encontrado";
                      return View("Error");
                  }

                  // Una lista de las notificaciones
                  var usuarioClaims = await gestionUsuarios.GetClaimsAsync(usuario);
                  // GetRolesAsync returns the list of user Roles
                  var usuarioRoles = await gestionUsuarios.GetRolesAsync(usuario);

                  var model = new EditarUsuarioModelo
                  {
                      Id = usuario.Id,
                      Email = usuario.Email,
                      NombreUsuario = usuario.UserName,
                      ayudaPass = usuario.ayudaPass,
                      Notificaciones = usuarioClaims.Select(c =>c.Type + " : " + c.Value).ToList(),
                      Roles = usuarioRoles
                  };

                  return View(model);
              }

              [HttpPost]
              [Route("Administracion/EditarUsuario")]
              public async Task<IActionResult> EditarUsuario(EditarUsuarioModelo model)
              {
                  var usuario = await gestionUsuarios.FindByIdAsync(model.Id);

                  if (usuario == null)
                  {
                      ViewBag.ErrorMessage = $"Usuario con Id = {model.Id} no fue encontrado";
                      return View("Error");
                  }
                  else
                  {
                      usuario.Email = model.Email;
                      usuario.UserName = model.NombreUsuario;
                      usuario.ayudaPass = model.ayudaPass;

                      var resultado = await gestionUsuarios.UpdateAsync(usuario);

                      if (resultado.Succeeded)
                      {
                          return RedirectToAction("ListaUsuarios");
                      }

                      foreach (var error in resultado.Errors)
                      {
                          ModelState.AddModelError("", error.Description);
                      }

                      return View(model);
                  }
              }

              [HttpPost]
              [Route("Administracion/BorrarUsuario")]
              public async Task<IActionResult> BorrarUsuario(string id)
              {
                  var user = await gestionUsuarios.FindByIdAsync(id);

                  if (user == null)
                  {
                      ViewBag.ErrorMessage = $"Usuario con Id = {id} no fue encontrado";
                      return View("Error");
                  }
                  else
                  {
                      var resultado = await gestionUsuarios.DeleteAsync(user);

                      if (resultado.Succeeded)
                      {
                          return RedirectToAction("ListaUsuarios");
                      }

                      foreach (var error in resultado.Errors)
                      {
                          ModelState.AddModelError("", error.Description);
                      }

                      return View("ListaUsuarios");

                  }
              }

              [HttpGet]
              [Route("Administracion/GestionarRolesUsuario")]
              [Authorize(Policy = "EditarRolPolicy")]
              public async Task<IActionResult> GestionarRolesUsuario(string IdUsuario)
              {
                  ViewBag.IdUsuario = IdUsuario;

                  var user = await gestionUsuarios.FindByIdAsync(IdUsuario);

                  if (user == null)
                  {
                      ViewBag.ErrorMessage = $"El usuario con id = {IdUsuario} no se encontro";
                      return View("Error");
                  }

                  var model = new List<RolUsuarioModelo>();

                  foreach (var rol in gestionRoles.Roles)
                  {
                      var rolUsuarioModelo = new RolUsuarioModelo
                      {
                          RolId  = rol.Id,
                          RolNombre = rol.Name
                      };

                      if (await gestionUsuarios.IsInRoleAsync(user, rol.Name))
                      {
                          rolUsuarioModelo.EstaSeleccionado = true;
                      }
                      else
                      {
                          rolUsuarioModelo.EstaSeleccionado = false;
                      }

                      model.Add(rolUsuarioModelo);
                  }

                  return View(model);
              }

              [HttpPost]
              [Route("Administracion/GestionarRolesUsuario")]
              [Authorize(Policy = "EditarRolPolicy")]
              public async Task<IActionResult> GestionarRolesUsuario(List<RolUsuarioModelo> model, string IdUsuario)
              {
                  var usuario = await gestionUsuarios.FindByIdAsync(IdUsuario);

                  if (usuario == null)
                  {
                      ViewBag.ErrorMessage = $"El usuario con id = {IdUsuario} no se encontro";
                      return View("Error");
                  }

                  var roles = await gestionUsuarios.GetRolesAsync(usuario);
                  var result = await gestionUsuarios.RemoveFromRolesAsync(usuario, roles);

                  if (!result.Succeeded)
                  {
                      ModelState.AddModelError("", "No podemos borrar usuario con roles");
                      return View(model);
                  }

                  result = await gestionUsuarios.AddToRolesAsync(usuario,
                      model.Where(x => x.EstaSeleccionado).Select(y => y.RolNombre));

                  if (!result.Succeeded)
                  {
                      ModelState.AddModelError("", "No podemos añadir los roles al usuario seleccionados");
                      return View(model);
                  }

                  return RedirectToAction("EditarUsuario", new { Id = IdUsuario });
              }

              [HttpGet]
              [Route("Administracion/GestionarUsuarioClaims")]
              public async Task<IActionResult> GestionarUsuarioClaims(string IdUsuario)
              {
                  var usuario = await gestionUsuarios.FindByIdAsync(IdUsuario);

                  if (usuario == null)
                  {
                      ViewBag.ErrorMessage = $"El usuario con id = {IdUsuario} no se encontro";
                      return View("Error");
                  }

                  // Obtenemos todos los claims del usuario actual
                  var existingUserClaims = await gestionUsuarios.GetClaimsAsync(usuario);

                  var modelo = new UsuarioClaimsViewModel
                  {
                   idUsuario = IdUsuario
                  };

                  // Recorremos los calims de nuestra aplicacion
                  foreach (Claim claim in AlmacenClaims.todosLosClaims)
                  {
                      UsuarioClaim usuarioClaim = new UsuarioClaim
                      {
                        tipoClaim = claim.Type
                      };

                      // Si el usuario tiene el claim que estamos recorrriendo en este momento lo seleccionamos
                      if (existingUserClaims.Any(c => c.Type == claim.Type && c.Value=="true"))
                      {
                          usuarioClaim.estaSeleccioando = true;
                      }

                      modelo.Claims.Add(usuarioClaim);
                  }

                  return View(modelo);

              }

              [HttpPost]
              [Route("Administracion/GestionarUsuarioClaims")]
              public async Task<IActionResult> GestionarUsuarioClaims(UsuarioClaimsViewModel modelo)
              {
                  var usuario = await gestionUsuarios.FindByIdAsync(modelo.idUsuario);

                  if (usuario == null)
                  {
                      ViewBag.ErrorMessage = $"El usuario con id = {modelo.idUsuario} no se encontro";
                      return View("Error");
                  }

                  // Obtenemos los claims del usuario y los borramos
                  var claims = await gestionUsuarios.GetClaimsAsync(usuario);
                  var result = await gestionUsuarios.RemoveClaimsAsync(usuario, claims);

                  if (!result.Succeeded)
                  {
                      ModelState.AddModelError("", "No se pueden borrar los claims de este usuario");
                      return View(modelo);
                  }

                  // Volvemos a asociar los seleccioando en la inetrfaz gráfica
                  result = await gestionUsuarios.AddClaimsAsync(usuario,
                      modelo.Claims.Select(c => new Claim(c.tipoClaim, c.estaSeleccioando ? "true":"false")));

                  if (!result.Succeeded)
                  {
                      ModelState.AddModelError("", "No se pueden agregar claims a este usuario");
                      return View(modelo);
                  }

                  return RedirectToAction("EditarUsuario", new { Id = modelo.idUsuario });

              }*/

    }


}
