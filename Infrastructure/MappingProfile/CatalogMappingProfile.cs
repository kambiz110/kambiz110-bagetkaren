using Application.Banners;
using Application.Catalogs.CatalogCars.Dto;
using Application.Catalogs.CatalogCompany.Dto;
using Application.Catalogs.CatalogTypeImages;
using Application.Catalogs.CatalogTypes;
using Application.Catalogs.CatalohItems.AddNewCatalogItem;
using Application.Catalogs.CatalohItems.CatalogItemServices;
using Application.Catalogs.GetMenuItem;
using Application.Discounts.AddNewDiscountServices;
using Application.Discounts.Dto;
using AutoMapper;
using Domain.Banners;
using Domain.Catalogs;
using Domain.Discounts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.MappingProfile
{
    public class CatalogMappingProfile : Profile
    {
        public CatalogMappingProfile()
        {
            CreateMap<CatalogType, CatalogTypeDto>().ReverseMap();

            CreateMap<CatalogType, CatalogTypeListDto>()
                .ForMember(dest => dest.SubTypeCount, option =>
                 option.MapFrom(src => src.SubType.Count));

            CreateMap<CatalogType, MenuItemDto>()
                .ForMember(dest => dest.Name, opt =>
                 opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.ParentId, opt =>
                 opt.MapFrom(src => src.ParentCatalogTypeId))
                .ForMember(dest => dest.SubMenu, opt =>
                opt.MapFrom(src => src.SubType));
            //----------------------
            // پروفایل مپ افزودن مورد جدید
            CreateMap<CatalogItemFeature, AddNewCatalogItemFeature_dto>().ReverseMap();
            CreateMap<CatalogItemImage, AddNewCatalogItemImage_Dto>().ReverseMap();

            CreateMap<CatalogItem, AddNewCatalogItemDto>()
                .ForMember(dest => dest.Features, opt =>
                opt.MapFrom(src => src.CatalogItemFeatures))
                 .ForMember(dest => dest.Images, opt =>
                 opt.MapFrom(src => src.CatalogItemImages));

            CreateMap<AddNewCatalogItemDto, CatalogItem>();

            CreateMap<CatalogItem, CatalogItemListItemDto>()
                 .ForMember(dest => dest.Brand, opt =>
                opt.MapFrom(src => src.CatalogBrand.Brand))
                  .ForMember(dest => dest.Type, opt =>
                opt.MapFrom(src => src.CatalogType.Type));
            //-------------------
            CreateMap<CatalogBrand, CatalogBrandDto>().ReverseMap();
            CreateMap<CatalogType, CatalogTypeDto>().ReverseMap();

            //-----------------------
            CreateMap<CatalogCompany, CompanyDto>().ReverseMap();
            CreateMap<CatalogCompany, CompanyListDto>().ReverseMap();

            //--------------------------
            CreateMap<CatalogBrand, CatalogBrandDto>().ReverseMap();

 
  
            //----------------------------
            CreateMap<CatologCar, CarDto>().ReverseMap();
            CreateMap<CatologCar, CarListDto>().ReverseMap();
            //--------------------------------
            CreateMap<CatalogTypeImageDto, CatalogTypeImage>().ReverseMap();
        
        }
    }
}
