using Application.Catalogs.CatalogItems.UriComposer;
using Application.Dtos;
using Application.Interfaces.Contexts;
using AutoMapper;
using Domain.Banners;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Banners
{
  public  interface IGetBanerHomePage
    {
        BaseDto<List<BannerDto>> Executed(int position , int count);
    }
    public class GetBanerHomePage : IGetBanerHomePage
    {
        private readonly IDataBaseContext context;
        private readonly IMapper mapper;
        private readonly IUriComposerService uriComposerService;

        public GetBanerHomePage(IDataBaseContext context, IMapper mapper, IUriComposerService uriComposerService)
        {
            this.context = context;
            this.mapper = mapper;
            this.uriComposerService = uriComposerService;
        }
        public BaseDto<List<BannerDto>> Executed(int position, int count)
        {
            var data = context.Banners.AsNoTracking().Where(p => ((int)p.Position == position))
                .Take(count).AsQueryable();
           var result = mapper.ProjectTo<BannerDto>(data).ToList();
            for (int i = 0; i < result.Count; i++)
            {
                result.ElementAt(i).Image = uriComposerService.ComposeImageUri(result.ElementAt(i).Image);
            }
            
            return new BaseDto<List<BannerDto>>(
                true,
                null,
                result
                );
           
        }
    }
}
