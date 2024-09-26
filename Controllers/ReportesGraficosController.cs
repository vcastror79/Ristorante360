using Microsoft.AspNetCore.Mvc;
using Ristorante360Admin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Collections.Generic;
using Ristorante360Admin.Services.Contract;

namespace PruebaRistorante.Controllers
{
    [Authorize]
    public class ReportesGraficosController : Controller
    {
        private readonly RistoranteContext _ristoranteContext;
        private readonly IErrorLoggingService _errorLoggingService; // Asegúrate de inyectar el servicio de registro de errores

        public ReportesGraficosController(RistoranteContext ristoranteContext, IErrorLoggingService errorLoggingService)
        {
            _ristoranteContext = ristoranteContext;
            _errorLoggingService = errorLoggingService;
        }

        public IActionResult GraficosInventario()
        {
            try
            {
                // Obtener los datos de la tabla Inventory desde la base de datos
                var inventoryData = _ristoranteContext.Inventories.Include(i => i.oSupplies).Include(i => i.oUnitType).ToList();

                // Mapear los datos necesarios para el gráfico de Cantidad en inventario
                var labels = inventoryData.Select(item => item.oSupplies.Description).ToList();
                var dataPoints = inventoryData.Select(item => item.Amount).ToList();

                // Obtener el nivel mínimo de inventario para cada ítem
                var minInventory = inventoryData.Select(item => item.MinimumAmount).ToList();

                // Obtener la distribución del inventario por unidad
                var units = inventoryData.Select(item => item.oUnitType.Unit).Distinct().ToList();
                var inventoryByUnit = new List<int>();
                foreach (var unit in units)
                {
                    inventoryByUnit.Add(inventoryData.Where(item => item.oUnitType.Unit == unit).Sum(item => item.Amount));
                }

                // Configurar el modelo de datos del gráfico de Cantidad en inventario
                var chartData = new
                {
                    labels = labels,
                    datasets = new[]
                    {
                        new
                        {
                            label = "Cantidad en inventario",
                            backgroundColor = "rgba(75, 192, 192, 0.2)",
                            borderColor = "rgba(75, 192, 192, 1)",
                            borderWidth = 1,
                            data = dataPoints
                        }
                    }
                };

                // Configurar el modelo de datos del gráfico de Nivel mínimo de inventario
                var minInventoryChartData = new
                {
                    labels = labels,
                    datasets = new[]
                    {
                        new
                        {
                            label = "Nivel mínimo de inventario",
                            backgroundColor = "rgba(255, 99, 132, 0.2)",
                            borderColor = "rgba(255, 99, 132, 1)",
                            borderWidth = 1,
                            data = minInventory
                        }
                    }
                };

                // Configurar el modelo de datos del gráfico de Distribución del inventario por unidad
                var unitChartData = new
                {
                    labels = units,
                    datasets = new[]
                    {
                        new
                        {
                            label = "Distribución del inventario por unidad",
                            backgroundColor = new string[]
                            {
                                "rgba(255, 99, 132, 0.2)",
                                "rgba(54, 162, 235, 0.2)",
                                "rgba(255, 206, 86, 0.2)",
                                // Agregar más colores de fondo para cada unidad si hay más unidades
                            },
                            borderColor = new string[]
                            {
                                "rgba(255, 99, 132, 1)",
                                "rgba(54, 162, 235, 1)",
                                "rgba(255, 206, 86, 1)",
                                // Agregar más colores de borde para cada unidad si hay más unidades
                            },
                            borderWidth = 1,
                            data = inventoryByUnit
                        }
                    }
                };

                // Enviar los datos a la vista
                ViewData["ChartData"] = chartData;
                ViewData["MinInventoryChartData"] = minInventoryChartData;
                ViewData["UnitChartData"] = unitChartData;

                return View();
            }
            catch (Exception ex)
            {
                // Manejo de errores, registra el error y muestra una vista de error
                _errorLoggingService.LogError("Error al generar los gráficos de inventario.", ex.ToString());
                return View();
            }
        }

        public IActionResult GraficosProductos()
        {
            try
            {
                // Obtener los datos de la tabla Productos desde la base de datos
                var inventoryData = _ristoranteContext.Products.Include(i => i.oCategory).ToList();

                // Calcular la cantidad de productos disponibles y no disponibles
                var disponiblesCount = inventoryData.Count(p => p.Availability);
                var noDisponiblesCount = inventoryData.Count(p => !p.Availability);

                // Obtener los nombres de los productos disponibles y no disponibles
                var disponibles = inventoryData.Where(p => p.Availability).Select(p => p.ProductName).ToList();
                var noDisponibles = inventoryData.Where(p => !p.Availability).Select(p => p.ProductName).ToList();

                // Obtener los nombres y precios de los productos
                var productPrices = inventoryData.Select(p => new { ProductName = p.ProductName, Price = p.Price }).ToList();

                // Pasar los datos a la vista
                ViewData["DisponiblesCount"] = disponiblesCount;
                ViewData["NoDisponiblesCount"] = noDisponiblesCount;
                ViewData["Disponibles"] = disponibles;
                ViewData["NoDisponibles"] = noDisponibles;
                ViewData["ProductPrices"] = productPrices;

                return View();
            }
            catch (Exception ex)
            {
                // Manejo de errores, registra el error y muestra una vista de error
                _errorLoggingService.LogError("Error al generar los gráficos de productos.", ex.ToString());
                return View();
            }
        }

        public IActionResult Logs()
        {
            try
            {
                List<Log> logList = _ristoranteContext.Logs.Include(l => l.User).ToList();
                return View(logList);
            }
            catch (Exception ex)
            {
                // Manejo de errores, registra el error y muestra una vista de error
                _errorLoggingService.LogError("Error al obtener los registros de log.", ex.ToString());
                return View();
            }
        }
    }
}
