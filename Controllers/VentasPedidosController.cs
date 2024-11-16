using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ristorante360Admin.Models;
using Ristorante360Admin.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Ristorante360Admin.Services.Contract;
using System.Security.Claims;
using Ristorante360Admin.Services.Implementation;

namespace PruebaRistorante360.Controllers
{
    [Authorize]

    public class VentasPedidosController : Controller
    {
        private readonly ApplicationDbContext _ristorante360Context;
        private readonly ILogService _logService;
        private readonly ProcedureExecutor _procedureExecutor;
        private readonly IErrorLoggingService _errorLoggingService;


        public VentasPedidosController(ApplicationDbContext ristoranteContext, ILogService logService, ProcedureExecutor procedureExecutor, IErrorLoggingService errorLoggingService)
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
                    productos = _ristorante360Context.Products.Where(c => c.Availability == true).ToList();
                }
                else
                {
                    productos = _ristorante360Context.Products.Where(p => p.CategoryId == categoryId).ToList();
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

        public IActionResult Pedidos()
        {
            try
            {
                DateTime oneDayAgo = DateTime.Now.AddDays(-1);

                // Obtén las órdenes de la base de datos, asegurándote de que la lista no sea null
                var ordersWithClientsAndProducts = _ristorante360Context.Orders
                    .Include(o => o.Client)
                    .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                    .Where(o =>
                        o.OrderStatusId == 1 ||
                        o.OrderStatusId == 2 ||
                        o.OrderStatusId == 3 ||
                        (o.OrderStatusId == 4 && o.OrderDate >= oneDayAgo))
                    .ToList() ?? new List<Order>(); // Devuelve una lista vacía si el resultado es null

                // Validar los datos de los clientes para evitar valores nulos o vacíos
                foreach (var order in ordersWithClientsAndProducts)
                {
                    if (order.Client != null) // Si la orden tiene un cliente asociado
                    {
                        order.Client.FullName = !string.IsNullOrWhiteSpace(order.Client.FullName)
                            ? order.Client.FullName
                            : "Cliente no especificado";

                        order.Client.Address = !string.IsNullOrWhiteSpace(order.Client.Address)
                            ? order.Client.Address
                            : "Dirección no especificada";
                    }
                    else
                    {
                        // Si la orden no tiene un cliente asociado, asignar valores predeterminados
                        order.Client = new Client
                        {
                            FullName = "Cliente no especificado",
                            Address = "Dirección no especificada"
                        };
                    }
                }

                // Cargar los estados de las órdenes
                var orderStatusesFromDb = _ristorante360Context.Set<OrderStatus>().FromSqlRaw("SELECT * FROM Order_Status").ToList();
                var statusDictionary = orderStatusesFromDb.ToDictionary(s => s.OrderStatusId, s => s.Description);
                ViewBag.OrderStatuses = statusDictionary;

                return View(ordersWithClientsAndProducts);
            }
            catch (Exception ex)
            {
                _errorLoggingService.LogError("Ocurrió un error al cargar la lista de pedidos.", ex.ToString());
                return View("Error");
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

                if (categoryId == 0) // Mostrar todos los productos
                {
                    productos = _ristorante360Context.Products.ToList();
                }
                else // Filtrar por categoría
                {
                    productos = _ristorante360Context.Products.Where(p => p.CategoryId == categoryId).ToList();
                }

                return View(productos);
            }
            catch (Exception ex)
            {
                // Manejo de errores
                string errorMessage = "Ocurrió un error al obtener los detalles de los productos.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);
                return View("Error");
            }
        }

        [HttpPost]
        public IActionResult AgregarOrden([FromBody] List<OrderProduct> orderProducts)
        {
            try
            {
                // Obtener el UserId del usuario autenticado
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Validar si el UserId es null o no es un número válido
                if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int parsedUserId))
                {
                    return Json(new { success = false, message = "Error: el usuario no está autenticado correctamente." });
                }

                // Crear un nuevo objeto Order y establecer sus valores
                var newOrder = new Order
                {
                    OrderDate = DateTime.Now,
                    OrderStatusId = 1, // Estado inicial de la orden
                    UserId = parsedUserId // Asigna el UserId obtenido del usuario autenticado
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





        ///Metodo combinado
        [HttpPost]
        public IActionResult UpdateOrderStatus(int orderId, int statusId)
        {
            using var transaction = _ristorante360Context.Database.BeginTransaction();

            try
            {
                // Buscar la orden
                var order = _ristorante360Context.Orders.FirstOrDefault(o => o.OrderId == orderId);
                if (order == null)
                {
                    return Json(new { success = false, message = "Orden no encontrada." });
                }

                // Verificar si el estado es "Completado" (estadoId = 3)
                if (statusId == 3)
                {
                    // Ejecutar DescontarInsumos para ajustar el inventario
                    _procedureExecutor.ExecuteDescontarInsumos(orderId);

                    // Después de descontar insumos, ejecutar CreateNotificationForAgotado
                    var createDate = DateTime.Now;
                    _procedureExecutor.ExecuteCreateNotificationForAgotado(createDate);
                }

                // Actualizar el estado de la orden
                order.OrderStatusId = statusId;
                _ristorante360Context.SaveChanges();
                transaction.Commit(); // Confirmar la transacción

                // Devolver una respuesta exitosa
                return Json(new { success = true, newStatusText = ObtenerDescripcionEstado(statusId) });
            }
            catch (Exception ex)
            {
                // Revertir la transacción en caso de error
                transaction.Rollback();

                // Registrar el error
                string errorMessage = $"Error al actualizar el estado de la orden #{orderId} a estado {statusId}.";
                _errorLoggingService.LogError(errorMessage, ex.ToString());

                // Devolver un JSON con el mensaje de error
                return Json(new { success = false, message = $"Ocurrió un error al actualizar el estado: {ex.Message}" });
            }
        }



        // Método auxiliar para obtener la descripción del estado según el ID
        private string ObtenerDescripcionEstado(int statusId)
        {
            return _ristorante360Context.OrderStatuses
                       .Where(s => s.OrderStatusId == statusId)
                       .Select(s => s.Description)
                       .FirstOrDefault() ?? "Estado desconocido";
        }



    }
}


