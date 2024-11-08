using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ristorante360Admin.Controllers
{

    [Authorize]
    public class AcercaDeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
