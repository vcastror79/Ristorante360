using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp; // Asegúrate de que esta referencia esté presente
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing; // Para Mutate y Resize
using Ristorante360Admin.Models;
using Ristorante360Admin.Models.ViewModels; // Para manejar formatos de imagen
using Ristorante360Admin.Services.Contract;
using Ristorante360Admin.Services.Implementation;

namespace Ristorante360Admin.Controllers
{
    [Authorize]
    public class ProductosController : Controller
    {

        private readonly ApplicationDbContext _ristoranteContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ILogService _logService;
        private readonly IErrorLoggingService _errorLoggingService;

        public ProductosController(ApplicationDbContext ristorante360Context, IWebHostEnvironment hostEnvironment, ILogService logService, IErrorLoggingService errorLoggingService)
        {
            _ristoranteContext = ristorante360Context;
            webHostEnvironment = hostEnvironment;
            _logService = logService;
            _errorLoggingService = errorLoggingService;

        }

        public IActionResult ProductList()
        {
            try
            {
                // Recupera una lista de productos desde la base de datos, incluyendo la información de categoría para cada producto.
                List<Product> listProduct = _ristoranteContext.Products.Include(c => c.oCategory).ToList();

                // Devuelve una vista llamada "ProductList" y pasa la lista de productos como modelo.
                return View(listProduct);
            }
            catch (Exception ex)
            {
                string errorMessage = "Ocurrió un error al obtener la lista de productos.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);

                ViewData["Message"] = "Se produjo un error al obtener la lista de productos.";
                return View(); // Devuelve la vista con el mensaje de error.
            }
        }

        public IActionResult ProductEdit()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ProductCreate(int productId)
        {
            try
            {
                ProductVM oProductVM = new ProductVM()
                {
                    oProduct = new Product(),
                    oListCategory = _ristoranteContext.Categories.Select(Category => new SelectListItem()
                    {
                        Text = Category.CategoryName,
                        Value = Category.CategoryId.ToString()
                    }).ToList()
                };

                if (productId != 0)
                {
                    oProductVM.oProduct = _ristoranteContext.Products.Find(productId);
                }

                return View(oProductVM);
            }
            catch (Exception ex)
            {
                string errorMessage = "Ocurrió un error al procesar la página de creación de productos.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);

                ViewData["Message"] = "Se produjo un error al obtener los datos del producto.";
                return View();
            }
        }


        [HttpPost]
        public IActionResult ProductCreate(ProductVM oProductVM)
        {
            try
            {
                bool isNewProduct = oProductVM.oProduct.ProductId == 0;
                var product = isNewProduct ? new Product() : _ristoranteContext.Products.Find(oProductVM.oProduct.ProductId);

                if (product == null && !isNewProduct)
                {
                    // Manejar el caso en que no se encuentre el producto original
                    throw new Exception("Producto no encontrado");
                }

                if (oProductVM.oProduct.ImageFile != null)
                {
                    string uniqueFileName = SaveProductImage(oProductVM, webHostEnvironment);
                    product.Image = uniqueFileName;
                }

                if (isNewProduct || product.ProductName != oProductVM.oProduct.ProductName)
                    product.ProductName = oProductVM.oProduct.ProductName;

                if (isNewProduct || product.ProductDescription != oProductVM.oProduct.ProductDescription)
                    product.ProductDescription = oProductVM.oProduct.ProductDescription;

                if (isNewProduct || product.Price != oProductVM.oProduct.Price)
                    product.Price = oProductVM.oProduct.Price;

                if (isNewProduct || product.Availability != oProductVM.oProduct.Availability)
                    product.Availability = oProductVM.oProduct.Availability;

                if (isNewProduct || product.CategoryId != oProductVM.oProduct.CategoryId)
                    product.CategoryId = oProductVM.oProduct.CategoryId;

                if (isNewProduct)
                {
                    _ristoranteContext.Products.Add(product);
                }

                _ristoranteContext.SaveChanges();
                _logService.Log($"Se {(isNewProduct ? "creó" : "actualizó")} el producto: {product.ProductName}", "Productos");

                return RedirectToAction("ProductList", "Productos");
            }
            catch (Exception ex)
            {
                string errorMessage = "Ocurrió un error al guardar los datos del producto.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);

                ViewData["Message"] = "Se produjo un error al guardar los datos del producto.";
                return View();
            }
        }

        [HttpGet]
        public IActionResult ProductDelete(int productId)
        {
            try
            {
                Product oProduct = _ristoranteContext.Products.Include(c => c.oCategory).Where(u => u.ProductId == productId).FirstOrDefault();

                if (oProduct == null)
                {
                    throw new Exception("Producto no encontrado");
                }

                return View(oProduct);
            }
            catch (Exception ex)
            {
                string errorMessage = "Ocurrió un error al obtener los datos del producto para eliminar.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);

                ViewData["Message"] = "Se produjo un error al obtener los datos del producto para eliminar.";
                return View();
            }
        }

        [HttpPost]
        public IActionResult ProductDelete(Product oProduct)
        {
            try
            {
                Product productToDelete = _ristoranteContext.Products.Find(oProduct.ProductId);

                if (productToDelete == null)
                {
                    throw new Exception("Producto no encontrado");
                }

                // Eliminar la imagen asociada al producto de la ruta 
                DeleteProductImage(productToDelete.Image, webHostEnvironment);

                _ristoranteContext.Products.Remove(productToDelete);
                _ristoranteContext.SaveChanges();

                _logService.Log($"Se eliminó el producto con ID: {oProduct.ProductId}, llamado {oProduct.ProductName}", "Productos");

                return RedirectToAction("ProductList", "Productos");
            }
            catch (Exception ex)
            {
                string errorMessage = "Ocurrió un error al eliminar el producto.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);

                ViewData["Message"] = "Se produjo un error al eliminar el producto.";
                return View();
            }
        }

        [HttpGet]
        public IActionResult SuppliesList()
        {
            try
            {
                List<Supply> listSupplies = _ristoranteContext.Supplies.ToList();

                return View(listSupplies);
            }
            catch (Exception ex)
            {
                string errorMessage = "Ocurrió un error al obtener la lista de insumos.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);

                ViewData["Message"] = "Se produjo un error al obtener la lista de insumos.";
                return View();
            }
        }

        [HttpPost]
        public IActionResult CreateSupply(string descripcion)
        {
            try
            {
                if (!string.IsNullOrEmpty(descripcion))
                {
                    // Verificar si la descripción ya existe en la base de datos
                    bool descripcionExistente = _ristoranteContext.Supplies.Any(s => s.Description == descripcion);

                    if (!descripcionExistente)
                    {
                        Supply supply = new Supply()
                        {
                            Description = descripcion
                        };

                        _ristoranteContext.Supplies.Add(supply);
                        _ristoranteContext.SaveChanges();
                        _logService.Log($"Se crea insumo: {descripcion}", "Productos");

                        return RedirectToAction("SuppliesList", "Productos");
                    }
                    else
                    {
                        TempData["ErrorMessageSuppy"] = "La descripción del insumo ya existe en la base de datos. Favor verifique nuevamente.";
                    }
                }
                else
                {
                    TempData["ErrorMessageSuppy"] = "La descripción del insumo es requerida.";
                }

                return RedirectToAction("SuppliesList", "Productos");
            }
            catch (Exception ex)
            {
                string errorMessage = "Ocurrió un error al crear un insumo.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);

                ViewData["Message"] = "Se produjo un error al crear un insumo.";
                return View();
            }
        }


        [HttpPost]
        public IActionResult DeleteSupply(int suppliesId)
        {
            try
            {
                // Verifica si el insumo está parametrizado a un producto y obtén el registro relacionado
                var supplyForProduct = _ristoranteContext.SuppliesForProducts.FirstOrDefault(sfp => sfp.SuppliesId == suppliesId);

                if (supplyForProduct != null)
                {
                    // El insumo está parametrizado a un producto, obtén información del producto
                    int productId = supplyForProduct.ProductId;
                    var product = _ristoranteContext.Products.FirstOrDefault(p => p.ProductId == productId);

                    if (product != null)
                    {
                        // Aquí puedes acceder a la información del producto
                        string productName = product.ProductName;
                        TempData["MessageInsumo"] = "Este insumo está parametrizado al producto: " + productName;
                        return RedirectToAction("SuppliesList");
                    }
                }

                // Si el insumo no está parametrizado a un producto, continúa con la eliminación
                Supply supplyDelete = _ristoranteContext.Supplies.Find(suppliesId);

                if (supplyDelete != null)
                {
                    _ristoranteContext.Supplies.Remove(supplyDelete);
                    _ristoranteContext.SaveChanges();

                }

                _logService.Log($"Se elimina el insumo con ID: {suppliesId}", "Productos");

                return RedirectToAction("SuppliesList", "Productos");
            }
            catch (Exception ex)
            {
                string errorMessage = "Ocurrió un error al eliminar un insumo.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);

                ViewData["Message"] = "Se produjo un error al eliminar el insumo.";
                return View();
            }
        }



        #region GUARDAR Y ELIMINAR IMAGEN DE PRODUCTOS

        //Metodo para guardar imagen de productos
        private string SaveProductImage(ProductVM oProductVM, IWebHostEnvironment webHostEnvironment)
        {
            try
            {
                string uniqueFileName = null;

                if (oProductVM.oProduct.ImageFile != null)
                {
                    string imageUploadedFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images/Product");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + oProductVM.oProduct.ImageFile.FileName;
                    string filepath = Path.Combine(imageUploadedFolder, uniqueFileName);

                    // Declara explícitamente el tipo de la imagen
                    using (Image image = Image.Load(oProductVM.oProduct.ImageFile.OpenReadStream()))
                    {
                        image.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Size = new Size(231, 231),
                            Mode = ResizeMode.Crop
                        }));

                        image.Save(filepath); // Guarda la imagen en el filepath
                    }

                    oProductVM.oProduct.Image = uniqueFileName;
                }

                return uniqueFileName;
            }
            catch (Exception ex)
            {
                string errorMessage = "Ocurrió un error al guardar la imagen del producto.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);
                throw;
            }
        }

        private void DeleteProductImage(string imageName, IWebHostEnvironment webHostEnvironment)
        {
            try
            {
                if (!string.IsNullOrEmpty(imageName))
                {
                    string imagePath = Path.Combine(webHostEnvironment.WebRootPath, "Images/Product", imageName);

                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMessage = "Ocurrió un error al eliminar la imagen del producto.";
                string exceptionMessage = ex.ToString();
                _errorLoggingService.LogError(errorMessage, exceptionMessage);
            }
        }


        #endregion

    }
}
