﻿using Application.Catalogs.CatalohItems.CatalogItemServices;
using Application.Discounts;
using Application.Discounts.Dto;
using Application.Discounts.EditDiscountServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.EndPoint.Controllers
{
    public class DiscountController : Controller
    {
        private readonly IGetDescountesForAdmin getDescountes;
        private readonly IGetDescountForEdit descountForEdit;
        private readonly ICatalogItemService catalogItemService;
        private readonly IEDitDiscount editDiscount;

        public DiscountController(IGetDescountesForAdmin getDescountes, IGetDescountForEdit descountForEdit
            , ICatalogItemService catalogItemService , IEDitDiscount editDiscount)
        {
            this.getDescountes = getDescountes;
            this.descountForEdit = descountForEdit;
            this.catalogItemService = catalogItemService;
            this.editDiscount = editDiscount;
        }
        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            var model = getDescountes.GetListDescounts(page, pageSize);
            return View(model);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewData["Categories2"] = new SelectList(catalogItemService.GetCatalogType(), "Id", "Type");
            ViewData["Brands2"] = new SelectList(catalogItemService.GetBrand(), "Id", "Brand");
            ViewData["Cars2"] = new SelectList(catalogItemService.GetCares(), "Id", "Name");
            var model = descountForEdit.GetDescount(id);
            return View(model.Data);
        }
        [HttpPost]
        public IActionResult Edit(GetDescountsForEditViewModel dto)
        {
            var result = editDiscount.Execute(dto);
            if (!result)
            {
                ViewData["Categories2"] = new SelectList(catalogItemService.GetCatalogType(), "Id", "Type");
                ViewData["Brands2"] = new SelectList(catalogItemService.GetBrand(), "Id", "Brand");
                ViewData["Cars2"] = new SelectList(catalogItemService.GetCares(), "Id", "Name");
                var model = descountForEdit.GetDescount(dto.Id);
                return View(model.Data);
            }
            return RedirectToAction("index");
        }
    }
}
