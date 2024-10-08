﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ristorante360Admin.Models.ViewModels
{
    public class InventoryVM
    {

        public Inventory oInventory { get; set; }
        public List<SelectListItem>? oListSupplies { get; set; }
        public List<SelectListItem>? oListUnitTypes { get; set; }

    }
}
