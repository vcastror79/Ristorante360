using Microsoft.AspNetCore.Mvc;
using Ristorante360Admin.Models;
using Ristorante360Admin.Models.ViewModels;
using Ristorante360Admin.Resources;
using Ristorante360Admin.Services.Contract;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Common;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;//nuevo


namespace Ristorante360Admin.Controllers
{

    public class UserController : Controller
    {
        private readonly string urlDomain = "https://localhost:7097/";
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _ristoranteContext;
        private readonly IEmailService _emailService; // Agregamos el servicio de Email
        private readonly ILogService _logService;
        private readonly IErrorLoggingService _errorLoggingService;

        public UserController(IUserService userService, ApplicationDbContext ristorante360Context, IConfiguration configuration, IEmailService emailService, ILogService logService, IErrorLoggingService errorLoggingService)
        {
            _ristoranteContext = ristorante360Context;
            _userService = userService;
            _configuration = configuration;
            _emailService = emailService; // Inyectamos el servicio de Email
            _logService = logService;
            _errorLoggingService = errorLoggingService;

        }

        public IActionResult Login()
        {
            return View();
        }



        //temporal

        [HttpGet]
        public IActionResult GenerarHash(string nuevaContrasena)
        {
            if (string.IsNullOrEmpty(nuevaContrasena))
            {
                return Content("Por favor, proporciona una nueva contraseña como parámetro en la URL.");
            }

            string hashContrasena = Utilities.EncryptKey(nuevaContrasena);
            return Content($"El hash de la contraseña '{nuevaContrasena}' es: {hashContrasena}");
        }




















        [HttpPost]
        public async Task<IActionResult> Login(UserLoginVM user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                // Buscar el usuario por correo electrónico
                User user_found = await _ristoranteContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (user_found == null)
                {
                    ViewData["Message"] = "El usuario y/o contraseña son incorrectos.";
                    return View();
                }

                // Comparar la contraseña ingresada, encriptada con SHA-256, con la almacenada en la base de datos
                string encryptedPassword = Utilities.EncryptKey(user.Password);

                if (user_found.Password != encryptedPassword)
                {
                    ViewData["Message"] = "El usuario y/o contraseña son incorrectos.";
                    return View();
                }

                // Autenticar y configurar la sesión
                string usuarioNombre = user_found.Name;
                int usuarioRol = user_found.RoleId;
                int usuarioId = user_found.UserId;

                List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, user_found.Name),
            new Claim(ClaimTypes.Role, usuarioRol.ToString()),
            new Claim(ClaimTypes.NameIdentifier, usuarioId.ToString())
        };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    properties
                );

                HttpContext.Session.SetInt32("UserId", usuarioId);
                HttpContext.Session.SetString("Username", usuarioNombre);
                HttpContext.Session.SetInt32("Rol", usuarioRol);
                _logService.Log($"Inicio de sesión del usuario", "User");

                return RedirectToAction("Index", "Home");
            }
            catch (DbException dbEx)
            {
                _errorLoggingService.LogError("Error de base de datos durante el inicio de sesión", dbEx.Message);
                ViewData["Message"] = "Hubo un error de base de datos al intentar iniciar sesión.";
                return View();
            }
            catch (Exception ex)
            {
                _errorLoggingService.LogError("Error durante el inicio de sesión", ex.Message);
                ViewData["Message"] = "Se produjo un error al obtener los datos del usuario.";
                return View();
            }
        }



        public IActionResult Register()
        {
            // Recupera la cadena JSON del usuario de TempData
            var userJson = TempData["UserData"] as string;

            if (!string.IsNullOrEmpty(userJson))
            {
                // Deserializa la cadena JSON en un objeto UserLoginVM
                var user = JsonConvert.DeserializeObject<UserLoginVM>(userJson);

                string userEmail = user.Email;

                var userViewModel = new UserNewVM
                {
                    Email = userEmail
                };

                return View(userViewModel);
            }

            // Manejo de error si la cadena JSON no se encuentra en TempData
            return RedirectToAction("Login");
        }


        [HttpPost]
        public async Task<IActionResult> Register(User model)
        {
            ModelState.Remove("oRole");
            ModelState.Remove("Name");
            ModelState.Remove("Surname");

            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                // Recuperar el usuario existente de la base de datos
                var existingUser = await _ristoranteContext.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

                // Verificar si el usuario existe
                if (existingUser == null)
                {
                    // Retornar una vista de error o redirigir a una página de error
                    ViewData["Message"] = "El usuario no existe.";
                    return View("Error"); // Puedes crear una vista llamada "Error" para mostrar mensajes de error amigables
                }

                // Actualizar el campo de Status y RoleId del usuario existente
                existingUser.Status = model.Status;
                existingUser.RoleId = model.RoleId;

                // Verificar si el campo Password ha sido modificado antes de encriptar y actualizarlo
                if (!string.IsNullOrEmpty(model.Password))
                {
                    existingUser.Password = Utilities.EncryptKey(model.Password);
                }

                // Guardar los cambios en la base de datos
                await _ristoranteContext.SaveChangesAsync();

                TempData["RegisterSuccess"] = true;
                return RedirectToAction(nameof(Register));
            }
            catch (Exception ex)
            {
                // Capturar y registrar el error usando el servicio
                string errorMessage = "Se produjo un error al actualizar el usuario. Verifique nuevamente";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);

                // Redirigir a una página de error o mostrar un mensaje de error personalizado.
                ViewData["Message"] = "Ocurrió un error al actualizar el usuario.";
                return View("Error"); // Puedes crear una vista llamada "Error" para mostrar mensajes de error amigables
            }
        }


        public IActionResult RegisterUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(User model)
        {
            try
            {
                var user = await _ristoranteContext.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

                if (user != null)
                {
                    // El usuario ya existe, mostrar un mensaje de error
                    TempData["RegisterError"] = true;
                    ViewData["Message"] = "El usuario ya está registrado.";
                    return View();
                }

                // Validar modelo
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                // Encriptar la clave en SHA-256 antes de guardarla
                model.Password = Utilities.EncryptKey(model.Password);

                // Guardar el nuevo usuario en la base de datos
                User user_created = await _userService.SaveUser(model);

                if (user_created.UserId > 0)
                {
                    // El usuario se creó exitosamente, mostrar mensaje de éxito y redirigir
                    TempData["RegisterSuccess"] = true;
                    return RedirectToAction(nameof(RegisterUser));
                }

                // Hubo un problema al crear el usuario, mostrar un mensaje de error
                TempData["RegisterError"] = true;
                return View();
            }
            catch (Exception ex)
            {
                // Capturar y registrar el error usando el servicio
                string errorMessage = "Se produjo un error al registrar el usuario. Verifique nuevamente";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);

                // Redirigir a una página de error o mostrar un mensaje de error personalizado.
                ViewData["Message"] = "Ocurrió un error al registrar el usuario.";
                return View("Error");
            }
        }




        [HttpGet]
        public ActionResult StartRecovery()
        {
            RecoveryViewModel model = new RecoveryViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> StartRecovery(RecoveryViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                string token = Utilities.EncryptKey(Guid.NewGuid().ToString());

                var oUser = await _ristoranteContext.Users.Where(e => e.Email == model.Email).FirstOrDefaultAsync();

                if (oUser != null)
                {
                    oUser.TokenRecovery = token;
                    _ristoranteContext.Users.Update(oUser);
                    await _ristoranteContext.SaveChangesAsync();

                    // Envío de correo de recuperación
                    await _emailService.SendEmailAsync(oUser.Email, token);

                    ViewData["MessageRecovery"] = "El correo de recuperación fue enviado, verifique su correo electrónico";
                    return View();
                }

                // El usuario no existe, muestra un mensaje de error
                ViewData["MessageRecovery"] = "El usuario no existe o el correo electrónico es inválido";
                return View();
            }
            catch (Exception ex)
            {
                // Capturar y registrar el error usando el servicio
                string errorMessage = "Se produjo un error al iniciar el proceso de recuperación de contraseña.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);

                // Redirigir a una página de error o mostrar un mensaje de error personalizado.
                TempData["RecoveryError"] = true;
                return View("Error"); // Puedes crear una vista llamada "Error" para mostrar mensajes de error amigables
            }
        }

        public async Task<IActionResult> Recovery(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    // Si no se proporciona un token válido, redirige a la vista de inicio de sesión
                    return RedirectToAction("Login", "User");
                }

                var oUser = await _ristoranteContext.Users.FirstOrDefaultAsync(t => t.TokenRecovery == token);

                if (oUser == null)
                {
                    // Si el token no es válido o ha expirado, muestra un mensaje de error y redirige a la vista de inicio de sesión
                    ViewBag.ErrorToken = "El token ha expirado o es inválido.";
                    return RedirectToAction("Login", "User");
                }

                // Crea el modelo de vista para la recuperación de contraseña y establece el token
                var model = new RecoveryPasswordViewModel
                {
                    token = token
                };

                return View(model);
            }
            catch (Exception ex)
            {
                // Capturar y registrar el error usando el servicio
                string errorMessage = "Se produjo un error al procesar la recuperación de contraseña.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);

                // Redirigir a una página de error o mostrar un mensaje de error personalizado.
                return RedirectToAction("Error", "Home");
            }
        }


        [HttpPost]
        public async Task<IActionResult> Recovery(RecoveryPasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var oUser = await _ristoranteContext.Users.FirstOrDefaultAsync(t => t.TokenRecovery == model.token);

                if (oUser != null)
                {
                    string hashedPassword = Resources.Utilities.EncryptKey(model.Password);

                    oUser.Password = hashedPassword;
                    oUser.TokenRecovery = null;

                    _ristoranteContext.Update(oUser);
                    await _ristoranteContext.SaveChangesAsync();

                    ViewBag.MessageRecovery = "Contraseña modificada con éxito";
                    return View("Login");
                }
                else
                {
                    ViewBag.ErrorMessage = "No se encontró un usuario válido para realizar el cambio de contraseña.";
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                // Capturar y registrar el error usando el servicio
                string errorMessage = "Se produjo un error al modificar la contraseña.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);

                // Redirigir a una página de error o mostrar un mensaje de error personalizado.
                return RedirectToAction("Login", "User");
            }
        }
    }
}
