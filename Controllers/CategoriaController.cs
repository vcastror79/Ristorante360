using Ristorante360.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ristorante360.Controllers
{
    public class CategoriaController : Controller
    {
        //Conexion DB
        private readonly RistoranteContext? _ristorante360Context;

        public CategoriaController(RistoranteContext? Ristorante360Context)
        {
            _ristorante360Context = Ristorante360Context;
        }

        //Index Categoria
        public ActionResult IndexCategory()
        {
            Category categoria = new Category();
            return View();
        }

        //List Categoria
        public IActionResult CategoriaList()
        {
            var categoriaList = _ristorante360Context.Categories.ToList();

            return View(categoriaList);
        }

        /* List<SelectListItem> items = lst.ConvertAll(d =>
         {
             return new SelectListItem()
             {
                 Text = d.M
             }
         });*/

        //Create Category
        public IActionResult CategoriaCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CategoriaCreate(Category category)
        {
            if (ModelState.IsValid)
            {
                var newCategory = new Category
                {
                    CategoryId = category.CategoryId,
                    CategoryName = category.CategoryName
                };

                _ristorante360Context.Categories.Add(category);
                await _ristorante360Context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        //Edit Category
        public IActionResult CategoriaEdit(int id)
        {
            var categorydb = _ristorante360Context.Categories.Find(id);

            if (categorydb == null)
            {
                return NotFound();
            }

            var newCategory = new Category
            {
                CategoryId = categorydb.CategoryId,
                CategoryName = categorydb.CategoryName
            };

            return View(categorydb);
        }

        [HttpPost]
        public IActionResult CategoriaEdit(Category category)
        {
            if (ModelState.IsValid)
            {
                var categorydb = _ristorante360Context.Categories.Find(category);

                if (categorydb == null)
                {
                    return NotFound();
                }

                categorydb.CategoryId = categorydb.CategoryId;
                categorydb.CategoryName = categorydb.CategoryName;

                return RedirectToAction("Index");
            }

            return View(category);
        }

        //Delete Category
        public async Task<IActionResult> CategoriaDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _ristorante360Context.Categories.Find(id);

            _ristorante360Context.Categories.Remove(category);
            _ristorante360Context.SaveChanges();
            return RedirectToAction("Index");
        }


        //Details Category
        public IActionResult CategoriaDetails(int id)
        {
            var category = _ristorante360Context.Categories.Find(id);

            if (category == null)
            {
                return NotFound();
            }

            var newCategory = new Category
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName
            };

            return View(category);
        }

        

    }
}
