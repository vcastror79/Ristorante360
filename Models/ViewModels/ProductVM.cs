﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ristorante360Admin.Models.ViewModels
{
    public class ProductVM
    {
        public Product oProduct { get; set; }
        public List<SelectListItem> oListCategory { get; set; }
    }
}

