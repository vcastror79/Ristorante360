using Ristorante360Admin.Models;
using Ristorante360Admin.Models.ViewModels;
using Ristorante360Admin.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;

namespace Ristorante360Admin.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {
        private readonly ApplicationDbContext _ristoranteContext;
        private readonly ILogService _logService;
        private readonly IErrorLoggingService _errorLoggingService;

        public ClientesController(ApplicationDbContext ristoranteContext, ILogService logService, IErrorLoggingService errorLoggingService)
        {
            _ristoranteContext = ristoranteContext;
            _logService = logService;
            _errorLoggingService = errorLoggingService;
        }

        // Lista de Clientes (Read)
        public IActionResult ClientList()
        {
            try
            {
                List<Client> listClient = _ristoranteContext.Clients
                    .Where(client => client.status == true) // Mostrar solo clientes activos
                    .ToList();
                return View(listClient);
            }
            catch (Exception ex)
            {
                string errorMessage = "Ocurrió un error al obtener la lista de clientes.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);
                ViewData["Message"] = "Se produjo un error al obtener la lista de clientes.";
                return View(); // Redirigir a una vista de error personalizada si es necesario
            }
        }

        // Crear Cliente (Create y Update)
        [HttpGet]
        public IActionResult ClientCreate(int clientId = 0)
        {
            try
            {
                var client = new Client();

                if (clientId != 0)
                {
                    client = _ristoranteContext.Clients.Find(clientId);
                }

                return View(client);
            }
            catch (Exception ex)
            {
                string errorMessage = "Ocurrió un error al obtener los datos del cliente.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);
                ViewData["Message"] = "Se produjo un error al obtener los datos del cliente.";
                return View(); // Redirigir a una vista de error personalizada si es necesario
            }
        }

        // Guardar Cliente (POST) - Crear o Actualizar (Create y Update)
        [HttpPost]
        public IActionResult ClientCreate(Client client)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (client.ClientId == 0) // Si es un cliente nuevo
                    {
                        var clientNuevo = new Client
                        {
                            FullName = client.FullName,
                            PhoneNumber = client.PhoneNumber,
                            Address = client.Address,
                            Email = client.Email,
                            status = true // Cliente activo por defecto
                        };
                        _ristoranteContext.Clients.Add(clientNuevo);
                        ViewData["Message"] = "Cliente añadido correctamente.";
                    }
                    else // Si es una actualización de un cliente existente
                    {
                        var existingClient = _ristoranteContext.Clients.Find(client.ClientId);
                        if (existingClient != null)
                        {
                            // Actualiza solo los campos que cambian, sin alterar el campo "status"
                            existingClient.FullName = client.FullName;
                            existingClient.PhoneNumber = client.PhoneNumber;
                            existingClient.Address = client.Address;
                            existingClient.Email = client.Email;

                            // Asegúrate de no cambiar el "status" durante la edición a menos que sea necesario
                            _ristoranteContext.Clients.Update(existingClient);
                            ViewData["Message"] = "Cliente actualizado correctamente.";
                        }
                        else
                        {
                            ViewData["Message"] = "El cliente no pudo ser encontrado.";
                        }
                    }

                    _logService.Log("Se creó o actualizó un cliente", "Clientes");
                    _ristoranteContext.SaveChanges();
                    return RedirectToAction("ClientList");
                }
                else
                {
                    // Si la validación falla, vuelve a la vista con los errores
                    return View(client);
                }
            }
            catch (Exception ex)
            {
                string errorMessage = "Ocurrió un error al guardar el cliente.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);
                ViewData["Message"] = "Se produjo un error al guardar el cliente.";
                return View(client);
            }
        }


        // Editar Cliente (Update)
        [HttpGet]
        public IActionResult ClientEdit(int clientId)
        {
            try
            {
                var client = _ristoranteContext.Clients.Find(clientId);
                if (client != null)
                {
                    return View("ClientCreate", client); // Reutiliza la vista de crear para editar
                }
                else
                {
                    ViewData["Message"] = "El cliente no pudo ser encontrado.";
                    return RedirectToAction("ClientList");
                }
            }
            catch (Exception ex)
            {
                string errorMessage = "Ocurrió un error al obtener los datos del cliente.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);
                ViewData["Message"] = "Se produjo un error al obtener los datos del cliente.";
                return View("Error");
            }
        }

        // Eliminar Cliente (Delete)
        [HttpGet]
        public IActionResult ClientDelete(int clientId)
        {
            try
            {
                Client client = _ristoranteContext.Clients.Find(clientId);
                if (client != null)
                {
                    return View(client);
                }
                else
                {
                    ViewData["Message"] = "El cliente no pudo ser encontrado.";
                    return RedirectToAction("ClientList");
                }
            }
            catch (Exception ex)
            {
                string errorMessage = "Ocurrió un error al obtener los datos del cliente.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);
                ViewData["Message"] = "Se produjo un error al obtener los datos del cliente.";
                return View("Error");
            }
        }

        // Confirmar eliminación de cliente (POST)
        [HttpPost]
        public IActionResult ClientDelete(Client client)
        {
            try
            {
                Client clientToDelete = _ristoranteContext.Clients.Find(client.ClientId);
                if (clientToDelete != null)
                {
                    // Aquí puedes definir si deseas eliminarlo físicamente o cambiar su estado
                    clientToDelete.status = false; // Alternar estado a inactivo en lugar de borrarlo físicamente
                    _ristoranteContext.Clients.Update(clientToDelete);
                    _ristoranteContext.SaveChanges();
                    _logService.Log($"Se eliminó el cliente con ID: {client.ClientId} y llamado {client.FullName}", "Clientes");

                    return RedirectToAction("ClientList");
                }
                else
                {
                    ViewData["Message"] = "El cliente no pudo ser encontrado.";
                    return RedirectToAction("ClientList");
                }
            }
            catch (Exception ex)
            {
                string errorMessage = "Ocurrió un error al eliminar el cliente.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);
                ViewData["Message"] = "Se produjo un error al eliminar el cliente.";
                return View("Error");
            }
        }
    }
}
