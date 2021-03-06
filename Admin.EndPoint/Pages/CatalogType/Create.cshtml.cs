using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin.EndPoint.ViewModels.Catalogs;
using Application.Catalogs.CatalogTypeImages;
using Application.Catalogs.CatalogTypes;
using Application.Dtos;
using AutoMapper;
using Infrastructure.ExternalApi.ImageServer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Admin.EndPoint.Pages.CatalogType
{
    public class CreateModel : PageModel
    {
        private readonly ICatalogTypeService catalogTypeService;
        private readonly IMapper mapper;
        private readonly IImageUploadService imageUploadService;
        private readonly ICRUDCatalogTypeImage cRUDCatalogTypeImage;

        public CreateModel(ICatalogTypeService catalogTypeService, IMapper mapper,
            IImageUploadService imageUploadService , ICRUDCatalogTypeImage cRUDCatalogTypeImage)
        {
            this.catalogTypeService = catalogTypeService;
            this.mapper = mapper;
            this.imageUploadService = imageUploadService;
            this.cRUDCatalogTypeImage = cRUDCatalogTypeImage;
        }

        [BindProperty]
        public CatalogTypeViewModel CatalogType { get; set; } = new CatalogTypeViewModel { };
        public List<String> Message { get; set; } = new List<string>();
        [BindProperty]
        public List<IFormFile> Files { get; set; }
        public void OnGet(int? parentId)
        {
            CatalogType.ParentCatalogTypeId = parentId;
        }


        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var model = mapper.Map<CatalogTypeDto>(CatalogType);
       
            List<string> images = new List<string>();
            if (Files.Count > 0)
            {
                //Upload 
                var resultUpload = imageUploadService.Upload(Files);
                foreach (var item in resultUpload)
                {
                    images.Add(item);
                }
            }
            
            var result = catalogTypeService.Add(model);
            var uploadResult = cRUDCatalogTypeImage.Add(new CatalogTypeImageDto { Id=0,CatalogTypeId= result.Data.Id,Src= images.FirstOrDefault()});

    
            if (result.IsSuccess)
            {
                return RedirectToPage("index", new { parentid = CatalogType.ParentCatalogTypeId });
            }
            Message = result.Message;
            return Page();
        }
    }
}
