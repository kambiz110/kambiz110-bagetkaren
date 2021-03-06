using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Banners;
using Infrastructure.ExternalApi.ImageServer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Admin.EndPoint.Pages.Banners
{
    public class CreateModel : PageModel
    {
        private readonly IBannersService banners;
        private readonly IImageUploadService imageUploadService;
        public CreateModel(IBannersService banners,
            IImageUploadService imageUploadService)
        {
            this.banners = banners;
            this.imageUploadService = imageUploadService;
        }


        [BindProperty]
        public BannerDto Banner { get; set; }
        [BindProperty]
        public IFormFile BannerImage { get; set; }



        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (BannerImage != null && BannerImage.Length > 0)
            {
                var result = imageUploadService.Upload(new List<IFormFile> { BannerImage });
                if (result.Count > 0)
                {
                    Banner.Image = result.FirstOrDefault();
                    banners.AddBanner(Banner);
                }
                return RedirectToPage("Index");
            }
            Banner.Image = null;
            banners.AddBanner(Banner);
      
            return RedirectToPage("Index");

        }
    }
}
