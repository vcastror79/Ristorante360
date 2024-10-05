using Ristorante360Admin.Models;
using Ristorante360Admin.Models.ViewModels;
using Ristorante360Admin.Services.Contract;
using Ristorante360Admin.Services.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace Ristorante360Admin.Controllers
{
    [Authorize]

    public class InventarioController : Controller
    {
        private readonly ApplicationDbContext _ristoranteContext;
        private readonly ProcedureExecutor _procedureExecutor;
        private readonly ILogService _logService;
        private readonly IErrorLoggingService _errorLoggingService;

        public InventarioController(ApplicationDbContext ristoranteContext, ProcedureExecutor procedureExecutor, ILogService logService, IErrorLoggingService errorLoggingService)
        {
            _ristoranteContext = ristoranteContext;
            _procedureExecutor = procedureExecutor;
            _logService = logService;
            _errorLoggingService = errorLoggingService;

        }

        public IActionResult ListaInsumos()
        {
            return View();
        }
        public IActionResult AñadirEntrada()
        {
            return View();
        }
        public IActionResult ConsultaInventario()
        {
            try
            {
                _procedureExecutor.ExecuteCheckInventoryStatus();

                List<Inventory> ListInventoy = _ristoranteContext.Inventories.Include(i => i.oSupplies).Include(u => u.oUnitType).ToList();

                return View(ListInventoy);
            }
            catch (Exception ex)
            {
                string errorMessage = "Ocurrió un error al consultar el inventario.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);

                ViewData["ErrorMessage"] = "Ocurrió un error al consultar el inventario. Por favor, inténtelo nuevamente más tarde.";
                return View(); // Puedes redirigir a una vista de error personalizada si lo deseas
            }
        }

        [HttpGet]
        public IActionResult AgregarInsumo(int inventoryId)
        {
            try
            {
                InventoryVM oInventoryVM = new InventoryVM()
                {
                    oInventory = new Inventory(),
                    oListSupplies = _ristoranteContext.Supplies.Select(Supply => new SelectListItem()
                    {
                        Text = Supply.Description,
                        Value = Supply.SuppliesId.ToString()
                    }).ToList(),

                    oListUnitTypes = _ristoranteContext.UnitTypes.Select(Unit => new SelectListItem()
                    {
                        Text = Unit.Unit,
                        Value = Unit.UnitId.ToString()
                    }).ToList()
                };


                if (inventoryId != 0)
                {
                    oInventoryVM.oInventory = _ristoranteContext.Inventories.Find(inventoryId);
                }

                return View(oInventoryVM);
            }
            catch (Exception ex)
            {
                string errorMessage = "Ocurrió un error al obtener los datos del usuario.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);

                ViewData["ErrorMessage"] = "Ocurrió un error al obtener los datos del usuario. Por favor, inténtelo nuevamente más tarde.";
                return View(); // Puedes redirigir a una vista de error personalizada si lo deseas
            }
        }

        [HttpPost]
        public IActionResult AgregarInsumo(InventoryVM oInventoryVM)
        {
            try
            {
                if (oInventoryVM.oInventory.InventoryId == 0)
                {
                    _ristoranteContext.Inventories.Add(oInventoryVM.oInventory);
                }
                else
                {
                    _ristoranteContext.Inventories.Update(oInventoryVM.oInventory);
                }
                _ristoranteContext.SaveChanges();

                return RedirectToAction("ConsultaInventario", "Inventario");
            }
            catch (Exception ex)
            {
                string errorMessage = "Ocurrió un error al guardar los datos del insumo.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);

                ViewData["ErrorMessage"] = "Ocurrió un error al guardar los datos del insumo. Por favor, inténtelo nuevamente más tarde.";
                return View();
            }
        }

        [HttpGet]
        public IActionResult EditarInsumo(int inventoryId)
        {
            try
            {
                InventoryVM oInventoryVM = new InventoryVM()
                {
                    oInventory = new Inventory(),
                    oListSupplies = _ristoranteContext.Supplies.Select(Supply => new SelectListItem()
                    {
                        Text = Supply.Description,
                        Value = Supply.SuppliesId.ToString()
                    }).ToList()
                };

                if (inventoryId != 0)
                {
                    oInventoryVM.oInventory = _ristoranteContext.Inventories.Find(inventoryId);
                }

                return View(oInventoryVM);
            }
            catch (Exception ex)
            {
                string errorMessage = "Ocurrió un error al obtener los datos del insumo.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);

                ViewData["ErrorMessage"] = "Ocurrió un error al obtener los datos del insumo. Por favor, inténtelo nuevamente más tarde.";
                return View();
            }
        }

        public IActionResult SupplieProductList()
        {
            try
            {
                List<SuppliesForProduct> listSupplieProduct = _ristoranteContext.SuppliesForProducts.Include(i => i.oSupplies).Include(c => c.oProduct).Include(u => u.oUnitType).ToList();
                return View(listSupplieProduct);
            }
            catch (Exception ex)
            {
                string errorMessage = "Ocurrió un error al obtener la lista de productos de insumos.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);

                ViewData["ErrorMessage"] = "Ocurrió un error al obtener la lista de productos de insumos. Por favor, inténtelo nuevamente más tarde.";
                return View(new List<SuppliesForProduct>());
            }
        }

        [HttpGet]
        public IActionResult SupplieProductCreate(int supplyProductId)
        {
            try
            {
                var oSuppliesForProductVM = new SuppliesProductVM
                {
                    oSuppliesForProducto = new SuppliesForProduct(),
                    oListSupplies = _ristoranteContext.Supplies
                        .Select(supply => new SelectListItem
                        {
                            Text = supply.Description,
                            Value = supply.SuppliesId.ToString()
                        })
                        .ToList()
                };

                var oSuppliesProductVM = new SuppliesProductVM
                {
                    oSuppliesForProducto = new SuppliesForProduct(),
                    oListProduct = _ristoranteContext.Products
                        .Select(product => new SelectListItem
                        {
                            Text = product.ProductName,
                            Value = product.ProductId.ToString()
                        })
                        .ToList()
                };

                var oUnitTypeList = _ristoranteContext.UnitTypes
                   .Select(unitType => new SelectListItem
                   {
                       Text = unitType.Unit,
                       Value = unitType.UnitId.ToString()
                   })
                   .ToList();

                ViewBag.UnitTypeList = oUnitTypeList; // Store the list in ViewBag


                var viewModel = new SuppliesProductCreateViewModel
                {
                    SuppliesForProductVM = oSuppliesForProductVM,
                    SuppliesProductVM = oSuppliesProductVM
                };

                if (supplyProductId != 0)
                {
                    viewModel.SuppliesForProductVM.oSuppliesForProducto = _ristoranteContext.SuppliesForProducts.Find(supplyProductId);
                    var productSupply = viewModel.SuppliesForProductVM.oSuppliesForProducto.ProductId;
                    var resultado = _ristoranteContext.Products
                                        .Where(c => c.ProductId == productSupply)
                                        .Select(c => new { c.ProductId, c.ProductName })
                                        .FirstOrDefault();

                    ViewData["ProductName"] = resultado.ProductName;
                    ViewData["ProductId"] = resultado.ProductId;
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                string errorMessage = "Ocurrió un error al obtener los datos del producto de insumo.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);

                ViewData["ErrorMessage"] = "Ocurrió un error al obtener los datos del producto de insumo. Por favor, inténtelo nuevamente más tarde.";
                return View(new SuppliesProductCreateViewModel());
            }
        }

        [HttpPost]
        public IActionResult SupplieProductCreate(SuppliesProductCreateViewModel viewModel, int ProductId)
        {
            try
            {
                if (viewModel.SuppliesForProductVM.oSuppliesForProducto.SuppliesProductId == 0)
                {
                    var productId = viewModel.SuppliesProductVM.oSuppliesForProducto.ProductId;
                    var suppliesId = viewModel.SuppliesForProductVM.oSuppliesForProducto.SuppliesId;
                    var quantity = viewModel.SuppliesForProductVM.oSuppliesForProducto.Quantity;
                    var unitId = viewModel.SuppliesForProductVM.oSuppliesForProducto.UnitId;


                    _ristoranteContext.SuppliesForProducts.Add(new SuppliesForProduct
                    {
                        ProductId = productId,
                        SuppliesId = suppliesId,
                        Quantity = quantity,
                        UnitId = unitId
                    });
                }
                else
                {
                    var supplyProductId = viewModel.SuppliesForProductVM.oSuppliesForProducto.SuppliesProductId;
                    var suppliesId = viewModel.SuppliesForProductVM.oSuppliesForProducto.SuppliesId;
                    var quantity = viewModel.SuppliesForProductVM.oSuppliesForProducto.Quantity;
                    var unitId = viewModel.SuppliesForProductVM.oSuppliesForProducto.UnitId;


                    var existingEntity = _ristoranteContext.SuppliesForProducts.Find(supplyProductId);
                    if (existingEntity != null)
                    {
                        // La entidad existe en el contexto, actualiza sus propiedades
                        existingEntity.SuppliesId = suppliesId;
                        existingEntity.Quantity = quantity;
                        existingEntity.UnitId = unitId;
                        _ristoranteContext.Update(existingEntity);
                    }
                }

                _ristoranteContext.SaveChanges();
                _logService.Log($"Parametrización del insumo: {viewModel.SuppliesForProductVM.oSuppliesForProducto.SuppliesId} con el producto: {ProductId}", "Inventario");

                return RedirectToAction("SupplieProductList", "Inventario");
            }
            catch (Exception ex)
            {
                string errorMessage = "Ocurrió un error al guardar los datos del producto de insumo.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);

                ViewData["Message"] = "Ocurrió un error al guardar los datos del producto de insumo. Por favor, inténtelo nuevamente más tarde.";
                return View(viewModel);
            }
        }

        [HttpPost]
        public IActionResult SupplieProductDelete(int supplyProductId)
        {
            try
            {
                SuppliesForProduct suppliesForProductDelete = _ristoranteContext.SuppliesForProducts.Find(supplyProductId);
                if (suppliesForProductDelete != null)
                {
                    _ristoranteContext.SuppliesForProducts.Remove(suppliesForProductDelete);
                    _ristoranteContext.SaveChanges();
                }
                _logService.Log($"Se eliminó la parametrización del ID : {supplyProductId}", "Inventario");

                return RedirectToAction("SupplieProductList", "Inventario");
            }
            catch (Exception ex)
            {
                string errorMessage = "Ocurrió un error al eliminar la parametrización del producto de insumo.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);

                ViewData["Message"] = "Ocurrió un error al eliminar la parametrización del producto de insumo. Por favor, inténtelo nuevamente más tarde.";
                return View();
            }
        }

        [HttpPost]
        public IActionResult InventarioDelete(int inventaryId)
        {
            try
            {
                Inventory inventoryDelete = _ristoranteContext.Inventories.Find(inventaryId);
                if (inventoryDelete != null)
                {
                    _ristoranteContext.Inventories.Remove(inventoryDelete);
                    _ristoranteContext.SaveChanges();
                }
                _logService.Log($"Se eliminó del inventario el ID: {inventaryId}", "Inventario");

                return RedirectToAction("ConsultaInventario", "Inventario");
            }
            catch (Exception ex)
            {
                string errorMessage = "Ocurrió un error al eliminar el producto del inventario.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);

                ViewData["Message"] = "Ocurrió un error al eliminar el producto del inventario. Por favor, inténtelo nuevamente más tarde.";
                return View();
            }
        }


    }
}
