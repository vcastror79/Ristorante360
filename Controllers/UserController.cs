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

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginVM user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                // Verificar si el usuario existe en la base de datos
                var user_found = await _ristoranteContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

                if (user_found == null)
                {
                    ViewData["Message"] = "El usuario y/o contraseña son incorrectos.";
                    return View();
                }



                // Validar si la contraseña es correcta
               // string hashedPassword = Utilities.EncryptKey(user.Password);

               // if (user_found.Password != hashedPassword)
             //   {
              //      ViewData["Message"] = "El usuario y/o contraseña son incorrectos.";
             //       return View();
             //   }


                // Validar si la contraseña es temporal
                if (user_found.IsTemporaryPassword)
                {
                    // Serializar el usuario y redirigir a Register
                    var userJson = JsonConvert.SerializeObject(new UserLoginVM
                    {
                        Email = user_found.Email,
                        Password = user.Password
                    });

                    TempData["UserData"] = userJson;
                    return RedirectToAction("Register", "User");
                }

                // Validar si la cuenta está activa
                if (!user_found.Status)
                {
                    ViewData["Message"] = "Tu cuenta está desactivada. Por favor, contacta al administrador.";
                    return View();
                }

                // Crear los claims para autenticar al usuario
                List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user_found.Name),
            new Claim(ClaimTypes.Role, user_found.RoleId.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user_found.UserId.ToString())
        };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties properties = new AuthenticationProperties { AllowRefresh = true };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    properties
                );

                // Guardar información en la sesión
                HttpContext.Session.SetInt32("UserId", user_found.UserId);
                HttpContext.Session.SetString("Username", user_found.Name);
                HttpContext.Session.SetInt32("Rol", user_found.RoleId);

                _logService.Log($"Inicio de sesión del usuario {user_found.Email}", "User");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _errorLoggingService.LogError("Error durante el inicio de sesión", ex.Message);
                ViewData["Message"] = "Se produjo un error al procesar el inicio de sesión.";
                return View();
            }
        }


        public IActionResult Register()
        {
            // Validar si TempData contiene datos
            var userJson = TempData["UserData"] as string;

            if (!string.IsNullOrEmpty(userJson))
            {
                try
                {
                    // Deserializar el JSON en un objeto UserLoginVM
                    var user = JsonConvert.DeserializeObject<UserLoginVM>(userJson);

                    var userViewModel = new UserNewVM
                    {
                        Email = user.Email
                    };

                    return View(userViewModel);
                }
                catch (Exception ex)
                {
                    // Manejar posibles errores de deserialización
                    _errorLoggingService.LogError("Error al deserializar datos del usuario en Register", ex.Message);
                }
            }

            // Redirigir al login si no hay datos válidos en TempData
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
                    ViewData["Message"] = "El usuario no existe.";
                    return View("Error"); // Puedes crear una vista llamada "Error" para manejar este caso
                }

                // Actualizar los campos del usuario existente
                existingUser.Status = model.Status;
                existingUser.RoleId = model.RoleId;

                // Verificar si la contraseña fue ingresada y realizar actualizaciones
                if (!string.IsNullOrEmpty(model.Password))
                {
                    existingUser.Password = Utilities.EncryptKey(model.Password);
                    existingUser.IsTemporaryPassword = false;

                    // Guardar los cambios en la base de datos
                    await _ristoranteContext.SaveChangesAsync();

                    // Redirigir a Login con éxito
                    return RedirectToAction("Login", "User", new { success = true });
                }
                else
                {
                    // Si no se ingresó una contraseña, mostrar un mensaje en la vista actual
                    ViewData["Message"] = "Por favor, asegúrate de ingresar una nueva contraseña.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                _errorLoggingService.LogError("Se produjo un error al actualizar el usuario.", ex.ToString());
                ViewData["Message"] = "Ocurrió un error al actualizar el usuario. Intenta nuevamente.";
                return View("Error");
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

                ModelState.Remove("oRole");

                if (!ModelState.IsValid)
                {
                    // El modelo no es válido, volver a mostrar la vista con los mensajes de error
                    return View(model);
                }

                // Encriptar la clave en SHA256
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
                return View("Error"); // Puedes crear una vista llamada "Error" para mostrar mensajes de error amigables
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