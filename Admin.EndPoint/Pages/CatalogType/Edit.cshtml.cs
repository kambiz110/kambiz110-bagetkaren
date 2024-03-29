using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin.EndPoint.Helper;
using Admin.EndPoint.Helper.UploadFile;
using Admin.EndPoint.ViewModels.Catalogs;
using Application.Catalogs.CatalogTypeImages;
using Application.Catalogs.CatalogTypes;
using Application.Catalogs.CatalogTypes.Dto;
using Application.Users.Token;
using AutoMapper;
using Domain.Catalogs;
using Infrastructure.ExternalApi.ImageServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Admin.EndPoint.Pages.CatalogType
{
    [Authorize(Roles = "Administrator,Maneger")]
    public class EditModel : PageModel
    {
        private readonly ICatalogTypeService catalogTypeService;
        private readonly IMapper mapper;
        private readonly IUploadFile imageUploadService;
        private readonly ICRUDCatalogTypeImage cRUDCatalogTypeImage;
        private readonly IGetUserToken getUserToken;

        public EditModel(ICatalogTypeService catalogTypeService, IMapper mapper,
             IUploadFile imageUploadService,
             ICRUDCatalogTypeImage cRUDCatalogTypeImage
            , IGetUserToken getUserToken)
        {
            this.catalogTypeService = catalogTypeService;
            this.mapper = mapper;
            this.imageUploadService = imageUploadService;
            this.cRUDCatalogTypeImage = cRUDCatalogTypeImage;
            this.getUserToken = getUserToken;
        }


        [BindProperty]
        public CatalogTypeViewModel CatalogType { get; set; } = new CatalogTypeViewModel();
        public List<String> Message { get; set; } = new List<string>();
        [BindProperty]
        public List<IFormFile> Files { get; set; }
        public void OnGet(int Id)
        {
            var model = catalogTypeService.FindById(Id);
            if (model.IsSuccess)
                CatalogType = mapper.Map<CatalogTypeViewModel>(model.Data);
            Message = model.Message;
        }

        public IActionResult OnPost()
        {
            List<string> images = new List<string>();
            if (Files.Count > 0)
            {
                //Upload 
                var resultUpload = imageUploadService.UploadFileToServers(Files);
                foreach (var item in resultUpload.FileNameAddress)
                {
                    images.Add(item);
                }
            }
            var model = mapper.Map<CatalogTypeDto>(CatalogType);
            if (images.Count > 0)
            {
                var removeImage = cRUDCatalogTypeImage.RemoveCatalogTypeId(CatalogType.Id);
                var uploadResult = cRUDCatalogTypeImage.Add(new CatalogTypeImageDto { Id = 0, CatalogTypeId = CatalogType.Id, Src = images.FirstOrDefault() });
                model.CatalogTypeImage= mapper.Map(uploadResult.Data, new CatalogTypeImage() { });
            }
            
            var result = catalogTypeService.Edit(model);
           
           
            Message = result.Message;
            CatalogType = mapper.Map<CatalogTypeViewModel>(result.Data);
            return Page();
        }
    }
}
