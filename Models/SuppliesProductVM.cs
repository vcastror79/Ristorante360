﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ristorante360.Models.ViewModels
{
    public class SuppliesProductVM
    {
        public SuppliesForProduct oSuppliesForProducto { get; set; }

        public List<SelectListItem> oListSupplies { get; set; }
        public List<SelectListItem> oListProduct { get; set; }

    }
}
