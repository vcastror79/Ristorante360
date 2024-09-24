using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ristorante360.Models;
using Ristorante360.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Ristorante360.Services.Contract;
using System.Security.Claims;
using Ristorante360.Services.Implementation;

namespace PruebaElChante.Controllers
{
    [Authorize]

    public class VentasPedidosController : Controller
    {
        private readonly RistoranteContext _ristorante360Context;
        private readonly ILogService _logService;
        private readonly ProcedureExecutor _procedureExecutor;
        private readonly IErrorLoggingService _errorLoggingService;


        public VentasPedidosController(RistoranteContext elChanteContext, ILogService logService, ProcedureExecutor procedureExecutor, IErrorLoggingService errorLoggingService)
        {
            _ristorante360Context = ristoranteContext;
            _logService = logService;
            _procedureExecutor = procedureExecutor;
            _errorLoggingService = errorLoggingService;


        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult MenuProductos(int categoryId)
        {
            try
            {
                List<Product> productos;

                if (categoryId == 0)
                {
                    productos = _ristoranteContext.Products.Where(c => c.Availability == true).ToList();
                }
                else
                {
                    productos = _ristoranteContext.Products.Where(p => p.CategoryId == categoryId).ToList();
                }

                return View(productos);
            }
            catch (Exception ex)
            {
                string errorMessage = "Ocurrió un error al procesar el menú de productos.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);
                return View();
            }
        }


        public IActionResult ListaProductos()
        {
            return View();
        }

        [HttpGet]
        public IActionResult DetalleProducto(int productId)
        {
            try
            {
                var producto = _ristorante360Context.Products.Where(p => p.ProductId == productId).FirstOrDefault();
                return View(producto);
            }
            catch (Exception ex)
            {
                // Capturar y registrar el error usando el servicio
                string errorMessage = "Ocurrió un error al cargar los detalles del producto.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);

                // Redirigir a una página de error o mostrar un mensaje de error personalizado.
                return View();
            }
        }



        [HttpGet]
        public IActionResult Pedidos()
        {
            try
            {
                DateTime oneDayAgo = DateTime.Now.AddDays(-1);

                var ordersWithClientsAndProducts = _ristorante360Context.Orders
                    .Include(o => o.Client)
                    .Include(o => o.OrderProducts) // Cargar los productos asociados a cada pedido
                    .ThenInclude(op => op.Product) // Cargar la información del producto
                    .Where(o => o.OrderStatusId == 1 || o.OrderStatusId == 2 ||
                           (o.OrderStatusId == 3 || o.OrderStatusId == 4 && o.OrderDate >= oneDayAgo))
                    .ToList();

                // Obtener los estados de la orden desde la base de datos
                var orderStatusesFromDb = _ristorante360Context.OrderStatuses.ToList();

                // Construir el diccionario con los datos de la base de datos
                var statusDictionary = orderStatusesFromDb.ToDictionary(s => s.OrderStatusId, s => s.Description);

                // Asignar el diccionario a ViewBag para que esté disponible en la vista
                ViewBag.OrderStatuses = statusDictionary;

                return View(ordersWithClientsAndProducts);
            }
            catch (Exception ex)
            {
                // Capturar y registrar el error usando el servicio
                string errorMessage = "Ocurrió un error al cargar la lista de pedidos.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);

                // Redirigir a una página de error o mostrar un mensaje de error personalizado.
                return View("Error");
            }
        }




        [HttpGet]
        public IActionResult ConfirmaPedido(int orderId)
        {
            try
            {
                // Obtener el pedido con el orderId especificado
                var order = _ristorante360Context.Orders.FirstOrDefault(o => o.OrderId == orderId);

                if (order == null)
                {
                    // Si no se encontró el pedido, mostrar un mensaje de error o redirigir a una página de error
                    return NotFound();
                }

                // Obtener los productos asociados al pedido
                var orderProducts = _ristorante360Context.OrderProducts
                    .Include(op => op.Product) // Incluir la información del producto relacionado
                    .Where(op => op.OrderId == orderId)
                    .ToList();

                // Calcular el total del pedido sumando los precios de los productos
                decimal totalAmount = orderProducts.Sum(op => (decimal)op.Product.Price); // Aquí se realiza la conversión explícita a decimal

                // Agregar los datos al modelo
                var model = new Order
                {
                    OrderId = order.OrderId,
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    OrderProducts = orderProducts
                };

                // Asegúrate de obtener el userId desde algún lugar antes de esta llamada.
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _logService.Log($"Se confirmó el pedido con número de orden: {orderId}", "VentasPedidos");
                return View(model);
            }
            catch (Exception ex)
            {
                // Capturar y registrar el error usando el servicio
                string errorMessage = "Ocurrió un error al confirmar el pedido.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);

                // Redirigir a una página de error o mostrar un mensaje de error personalizado.
                return View("Error");
            }
        }


        [HttpGet]
        public IActionResult GetAllClients()
        {
            try
            {
                var clients = _ristorante360Context.Clients.ToList();
                return Json(clients);
            }
            catch (Exception ex)
            {
                // Capturar y registrar el error usando el servicio
                string errorMessage = "Ocurrió un error al obtener la lista de clientes.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);

                // Devolver un mensaje de error en formato JSON
                return Json(new { success = false, message = "Ocurrió un error al obtener la lista de clientes." });
            }
        }


        public IActionResult DetalleMenu(int categoryId)
        {
            try
            {
                _procedureExecutor.ExecuteUpdateProductAvailability();

                List<Product> productos;

                if (categoryId == 0)
                {
                    productos = _ristorante360Context.Products.ToList();
                }
                else
                {
                    productos = _ristorante360Context.Products.Where(p => p.CategoryId == categoryId).ToList();
                }

                return View(productos);
            }
            catch (Exception ex)
            {
                // Capturar y registrar el error usando el servicio
                string errorMessage = "Ocurrió un error al obtener los detalles de los productos.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);

                // Devolver una vista de error o un mensaje de error personalizado
                return View("Error"); // Puedes crear una vista llamada "Error" para mostrar mensajes de error amigables
            }
        }

        [HttpPost]
        public IActionResult AgregarOrden([FromBody] List<OrderProduct> orderProducts)
        {
            try
            {
                // Crear un nuevo objeto Order y establecer sus valores
                var newOrder = new Order
                {
                    OrderDate = DateTime.Now,
                    OrderStatusId = 1, // Puedes establecer el OrderStatusId como 1 aquí, o recuperarlo de la base de datos si es necesario
                    UserId = 4 // Aquí debes implementar la lógica para obtener el UserId del usuario que está iniciando sesión
                };

                // Agregar el nuevo objeto Order a la tabla "Order"
                _ristorante360Context.Orders.Add(newOrder);

                // Guardar los cambios en la base de datos para obtener el OrderId generado
                _ristorante360Context.SaveChanges();

                // Obtener el OrderId generado
                int orderId = newOrder.OrderId;

                // Obtener el valor máximo actual de Order_Product_Id en la base de datos
                int maxOrderProductId = 0;
                if (_ristorante360Context.OrderProducts.Any())
                {
                    maxOrderProductId = _ristorante360Context.OrderProducts.Max(op => op.OrderProductId);
                }

                // Generar los valores de Order_Product_Id y establecer el OrderId para los nuevos objetos OrderProduct
                foreach (var orderProduct in orderProducts)
                {
                    maxOrderProductId++;
                    orderProduct.OrderProductId = maxOrderProductId;
                    orderProduct.OrderId = orderId;
                }

                // Agregar los objetos a la tabla "Order_product"
                _ristorante360Context.OrderProducts.AddRange(orderProducts);

                // Guardar los cambios en la base de datos
                _ristorante360Context.SaveChanges();

                // Ejecutar el procedimiento almacenado para actualizar el monto total de la orden
                _ristorante360Context.Database.ExecuteSqlRaw("EXEC UpdateOrderTotalAmount @orderId", new SqlParameter("@orderId", orderId));

                // Guardar los cambios en la base de datos nuevamente después de ejecutar el procedimiento almacenado
                _ristorante360Context.SaveChanges();

                // Devolver el orderId junto con el mensaje de éxito
                return Json(new { success = true, message = "Orden agregada exitosamente", orderId = orderId });
            }
            catch (Exception ex)
            {
                // Capturar y registrar el error usando el servicio
                string errorMessage = "Error al agregar una orden.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);

                return Json(new { success = false, message = "Error al agregar la orden." });
            }
        }

        [HttpPost]
        public IActionResult CompletarOrden(Order orderclient)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Buscar si el cliente ya existe en la base de datos por su FullName y PhoneNumber
                    Client existingClient = _ristorante360Context.Clients.FirstOrDefault(c =>
                        c.FullName == orderclient.Client.FullName && c.PhoneNumber == orderclient.Client.PhoneNumber);

                    // Si el cliente no existe, agregarlo a la tabla Clients
                    if (existingClient == null)
                    {
                        _ristorante360Context.Clients.Add(orderclient.Client);
                        _ristorante360Context.SaveChanges(); // Guardar cambios en la base de datos

                        // Obtener el ID del cliente generado por la base de datos
                        int clientId = orderclient.Client.ClientId;

                        // Asignar el ID del cliente a la propiedad ClientId del objeto Order
                        orderclient.ClientId = clientId;
                    }
                    else
                    {
                        // Si el cliente ya existe, asignar su ID a la propiedad ClientId del objeto Order
                        orderclient.ClientId = existingClient.ClientId;
                    }

                    // Obtener la orden existente por el OrderId
                    Order existingOrder = _ristorante360Context.Orders.FirstOrDefault(o => o.OrderId == orderclient.OrderId);

                    // Si la orden existe, actualizar los campos OrderTypeId, PaymentMethodId y ClientId
                    if (existingOrder != null)
                    {
                        existingOrder.OrderSpecifications = orderclient.OrderSpecifications;
                        existingOrder.OrderTypeId = orderclient.OrderTypeId;
                        existingOrder.PaymentMethodId = orderclient.PaymentMethodId;
                        existingOrder.OrderDate = DateTime.Now; // Establecer la fecha y hora del pedido
                        existingOrder.ClientId = orderclient.ClientId; // Asignar el ClientId a la orden existente
                    }

                    // Guardar los cambios en la base de datos
                    _ristorante360Context.SaveChanges();
                    TempData["OrderCompleted"] = true;
                    TempData["OrderId"] = orderclient.OrderId;

                    _logService.Log($"Se completa la orden del cliente ID: {orderclient.ClientId}", "VentasPedidos");

                    // Redireccionar a la página de éxito u otra página deseada después de guardar los datos
                    return RedirectToAction("Pedidos"); // Cambia "Exito" por la acción o vista deseada
                }

                // Si hay algún error en la validación del modelo, regresar a la vista con los mensajes de error
                return View(orderclient);
            }
            catch (Exception ex)
            {
                // Capturar y registrar el error usando el servicio
                string errorMessage = "Error al completar una orden.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);

                // Redirigir a una página de error o mostrar un mensaje de error personalizado.
                return View("Error");
            }
        }

        [HttpPost]
        public IActionResult UpdateOrderStatus(int orderId, int statusId)
        {
            using var transaction = _ristorante360Context.Database.BeginTransaction(); // Inicia una transacción

            try
            {
                var order = _ristorante360Context.Orders.FirstOrDefault(o => o.OrderId == orderId);
                if (order != null)
                {
                    if (statusId == 3)
                    {
                        _procedureExecutor.ExecuteDescontarInsumos(orderId);
                        _procedureExecutor.ExecuteUpdateProductAvailability();
                        _procedureExecutor.ExecuteCheckInventoryStatus();

                        order.OrderStatusId = statusId;
                    }
                    else
                    {
                        order.OrderStatusId = statusId;
                    }
                    _logService.Log($"Se cambia el estado a {statusId}, de la orden #{orderId}", "VentasPedidos");

                    var createDate = DateTime.Now;
                    _procedureExecutor.ExecuteCreateNotificationForAgotado(createDate);

                    _ristorante360Context.SaveChanges();

                    transaction.Commit(); // Confirma la transacción si todas las operaciones fueron exitosas
                }

                return RedirectToAction("Pedidos");
            }
            catch (Exception ex)
            {
                // Revierte la transacción en caso de excepción
                transaction.Rollback();

                // Captura y registra el error
                string errorMessage = "Ocurrió un error al procesar la solicitud.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);

                // Redirigir a una página de error o mostrar un mensaje de error personalizado.
                return View("Error");
            }
        }



    }
}


