using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ristorante360.Models.ViewModels
{
    public class UserVM
    {
        public User oUser { get; set; }
        public List<SelectListItem> oListRole { get; set; }
    }
}
