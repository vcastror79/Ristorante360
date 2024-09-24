using Ristorante360.Models;
using Ristorante360.Models.ViewModels;
using Ristorante360.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ristorante360.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly RistoranteContext _ristoranteContext;
        private readonly ILogService _logService;
        private readonly IErrorLoggingService _errorLoggingService;


        public AdminController(RistoranteContext RistoranteContext, ILogService logService, IErrorLoggingService errorLoggingService)
        {
            _ristoranteContext = RistoranteContext;
            _logService = logService;
            _errorLoggingService = errorLoggingService;
        }

        public IActionResult UserAdmin(bool statusId)
        {
            try
            {
                List<User> listUser = _ristoranteContext.Users.Include(c => c.oRole).Where(u => u.Status == statusId).ToList();
                ViewData["statusId"] = statusId;

                return View(listUser);
            }
            catch (Exception ex)
            {
                string errorMessage = "Ocurrió un error al obtener la lista de usuarios.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);
                ViewData["Message"] = "Se produjo un error al obtener la lista de usuarios.";
                return View("Error"); // O redirigir a una vista de error personalizada
            }
        }


        public IActionResult UserEdit()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UserCreate(int userId)
        {
            try
            {
                UserVM oUserVM = new UserVM()
                {
                    oUser = new User(),
                    oListRole = _ristoranteContext.Roles.Select(Role => new SelectListItem()
                    {
                        Text = Role.RoleName,
                        Value = Role.RoleId.ToString()
                    }).ToList()
                };

                if (userId != 0)
                {
                    oUserVM.oUser = _ristoranteContext.Users.Find(userId);
                }

                return View(oUserVM);
            }
            catch (Exception ex)
            {
                string errorMessage = "Ocurrió un error al obtener los datos del usuario.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);

                ViewData["Message"] = "Se produjo un error al obtener los datos del usuario.";
                return View("Error"); // O redirigir a una vista de error personalizada
            }
        }

        [HttpPost]
        public IActionResult UserCreate(UserVM oUserVM)
        {
            try
            {
                ModelState.Remove("oListRole");
                ModelState.Remove("oUser.oRole");
                ModelState.Remove("oUser.Password");

                if (!ModelState.IsValid)
                {
                    UserVM oUserVM2 = new UserVM()
                    {
                        oUser = new User(),
                        oListRole = _ristoranteContext.Roles.Select(Role => new SelectListItem()
                        {
                            Text = Role.RoleName,
                            Value = Role.RoleId.ToString()
                        }).ToList()
                    };

                    return View(oUserVM2);
                }

                if (oUserVM.oUser.UserId == 0)
                {
                    string hashedPassword = Resources.Utilities.EncryptKey(oUserVM.oUser.Password);
                    oUserVM.oUser.Password = hashedPassword;
                    oUserVM.oUser.Status = true;
                    _ristoranteContext.Users.Add(oUserVM.oUser);
                }
                else if (oUserVM.oUser.Password == null)
                {
                    var existingUser = _ristoranteContext.Users.FirstOrDefault(u => u.UserId == oUserVM.oUser.UserId);

                    var usuarioEdit = new User
                    {
                        Name = oUserVM.oUser.Name,
                        Surname = oUserVM.oUser.Surname,
                        Email = oUserVM.oUser.Email,
                        RoleId = oUserVM.oUser.RoleId,
                        Status = true,
                        Password = existingUser.Password // Asegúrate de que existingUser contenga el usuario actual
                    };

                    // Adjuntar la entidad al contexto y marcarla como modificada
                    var entry = _ristoranteContext.Entry(usuarioEdit);
                    entry.State = EntityState.Modified;

                    _ristoranteContext.SaveChanges();


                }
                else
                {
                    string hashedPassword = Resources.Utilities.EncryptKey(oUserVM.oUser.Password);
                    oUserVM.oUser.Password = hashedPassword;
                    oUserVM.oUser.Status = true;
                    _ristoranteContext.Users.Update(oUserVM.oUser);
                }

                _logService.Log("Se creó o actualizó un usuario", "Admin");
                _ristoranteContext.SaveChanges();

                return RedirectToAction("UserAdmin", "Admin");
            }
            catch (Exception ex)
            {
                string errorMessage = "Ocurrió un error al guardar los datos del usuario.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);

                ViewData["Message"] = "Se produjo un error al guardar los datos del usuario.";
                return View("Error"); // O redirigir a una vista de error personalizada
            }
        }


        [HttpPost]
        public IActionResult UserDelete(int userId)
        {
            try
            {
                User userToUpdate = _ristoranteContext.Users.FirstOrDefault(u => u.UserId == userId);

                if (userToUpdate != null)
                {
                    userToUpdate.Status = !userToUpdate.Status; // Alternar el valor del campo Status
                    _ristoranteContext.SaveChanges();

                    _logService.Log($"Se eliminó el usuario con ID: {userId}", "Admin");
                    return RedirectToAction("UserAdmin", "Admin");
                }
                else
                {
                    ViewData["Message"] = "El usuario no pudo ser encontrado.";
                    return View("Error"); // O redirigir a una vista de error personalizada
                }
            }
            catch (Exception ex)
            {
                string errorMessage = "Ocurrió un error al eliminar el usuario.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);

                ViewData["Message"] = "Se produjo un error al eliminar el usuario.";
                return View("Error"); // O redirigir a una vista de error personalizada
            }
        }


    }
}

