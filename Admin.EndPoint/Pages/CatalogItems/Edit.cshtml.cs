using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Catalogs.CatalogItems.GetCatalogItemAdmin;
using Application.Catalogs.CatalohItems.AddNewCatalogItem;
using Application.Catalogs.CatalohItems.CatalogItemServices;
using Application.Dtos;
using Infrastructure.ExternalApi.ImageServer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Admin.EndPoint.Pages.CatalogItems
{
    public class EditModel : PageModel
    {
        private readonly IAddNewCatalogItemService addNewCatalogItemService;
        private readonly ICatalogItemService catalogItemService;
        private readonly IImageUploadService imageUploadService;
        private readonly IGetAdminEditCatalogItem getAdminEditCatalogItem;

        public EditModel(IAddNewCatalogItemService addNewCatalogItemService
            , ICatalogItemService catalogItemService
            , IImageUploadService imageUploadService 
            , IGetAdminEditCatalogItem getAdminEditCatalogItem)
        {
            this.addNewCatalogItemService = addNewCatalogItemService;
            this.catalogItemService = catalogItemService;
            this.imageUploadService = imageUploadService;
            this.getAdminEditCatalogItem = getAdminEditCatalogItem;
        }
        public SelectList Categories { get; set; }
        public SelectList Brands { get; set; }
        public SelectList Companes { get; set; }
        public SelectList Cars { get; set; }


        [BindProperty]
        public AddNewCatalogItemDto Data { get; set; }
        [BindProperty]
        public List<IFormFile> Files { get; set; }


        public void OnGet(int Id)
        {
            Data = getAdminEditCatalogItem.Execute(Id);
           ViewData["Cars"]  = new SelectList(catalogItemService.GetCares(), "Id", "Name");
            ViewData["Companes"] = new SelectList(catalogItemService.GetCompanes(), "Id", "Name");
            ViewData["Categories"]  = new SelectList(catalogItemService.GetCatalogType(), "Id", "Type");
            ViewData["Brands"]  = new SelectList(catalogItemService.GetBrand(), "Id", "Brand");
        }



        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new JsonResult(new BaseDto<int>(false, allErrors.Select(p => p.ErrorMessage).ToList(), 0));
            }
            for (int i = 0; i < Request.Form.Files.Count; i++)
            {
                var file = Request.Form.Files[i];
                Files.Add(file);
            }
            List<AddNewCatalogItemImage_Dto> images = new List<AddNewCatalogItemImage_Dto>();
            if (Files.Count > 0)
            {
                //Upload 
                var result = imageUploadService.Upload(Files);
                foreach (var item in result)
                {
                    images.Add(new AddNewCatalogItemImage_Dto { Src = item });
                }
            }
            Data.Images = images;
            var resultService = addNewCatalogItemService.Execute(Data);
            return Page();
        }
    }
}
