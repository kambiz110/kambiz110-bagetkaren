﻿using Application.HomePageService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.EndPoint.Models.ViewComponents
{
    public class GetCatalogIItemPLP : ViewComponent
    {
        private readonly IHomePageService homePageService;

        public GetCatalogIItemPLP(IHomePageService homePageService)
        {
            this.homePageService = homePageService;
        }
        public IViewComponentResult Invoke()
        {
            var model = homePageService.GetData();
            var viewName = $"~/Areas/Admin/Views/Shared/Components/Product/{this.ViewComponentContext.ViewComponentDescriptor.ShortName}.cshtml";
            return View(viewName, model);
        }
    }
}
