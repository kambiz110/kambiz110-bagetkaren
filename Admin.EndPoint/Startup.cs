using Admin.EndPoint.MappingProfiles;
using Application.Banners;
using Application.Catalogs.CatalogBrands;
using Application.Catalogs.CatalogCars.CrudService;
using Application.Catalogs.CatalogCompany.CrudService;
using Application.Catalogs.CatalogFeature.Command;
using Application.Catalogs.CatalogItems.GetCatalogItemAdmin;
using Application.Catalogs.CatalogItems.RemoveImage;
using Application.Catalogs.CatalogItems.UriComposer;
using Application.Catalogs.CatalogTypeImages;
using Application.Catalogs.CatalogTypes;
using Application.Catalogs.CatalohItems.AddNewCatalogItem;
using Application.Catalogs.CatalohItems.CatalogItemServices;
using Application.Discounts;
using Application.Discounts.AddNewDiscountServices;
using Application.Discounts.EditDiscountServices;
using Application.Interfaces.Contexts;
using Application.Visitors.GetTodayReport;
using FluentValidation;
using Infrastructure.ExternalApi.ImageServer;
using Infrastructure.IdentityConfigs;
using Infrastructure.MappingProfile;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence.Contexts;
using Persistence.Contexts.MongoContext;
using Persistence.Seeding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.EndPoint
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddControllers();
            services.AddSession();
            services.AddScoped<ICrudCompanyService, CrudCompanyService>();
            services.AddScoped<IGetTodayReportService, GetTodayReportService>();
            services.AddTransient(typeof(IMongoDbContext<>), typeof(MongoDbContext<>));
            services.AddTransient<ICatalogTypeService, CatalogTypeService>();
            services.AddTransient<IAddNewCatalogItemService,  AddNewCatalogItemService>();
            services.AddTransient<ICatalogItemService,  CatalogItemService>();
            services.AddTransient<IImageUploadService,  ImageUploadService>();
            services.AddTransient<IAddNewDiscountService,  AddNewDiscountService>();
            services.AddTransient<IDiscountService,  DiscountService>();
            services.AddTransient<IDiscountHistoryService,  DiscountHistoryService>();
            services.AddTransient<IBannersService,  BannersService>();
            services.AddTransient<IUriComposerService,  UriComposerService>();
            services.AddTransient<ICatalogBrandCrudService, CatalogBrandCrudService>();
            services.AddTransient<ICrudCarService, CrudCarService>();
            services.AddTransient<IGetAdminEditCatalogItem, GetAdminEditCatalogItem>();
            services.AddTransient<IRemoveFeacherService, RemoveFeacherService>();
            services.AddTransient<IGetDescountesForAdmin, GetDescountesForAdmin>();
            services.AddTransient<IGetDescountForEdit, GetDescountForEdit>();
            services.AddTransient<IDeletItemInDescount, DeletItemInDescount>();
            services.AddTransient<IDeleteImageService, DeleteImageService>();
            services.AddTransient<IEDitDiscount, EDitDiscount>();
            services.AddTransient<ICRUDCatalogTypeImage, CRUDCatalogTypeImage>();


            #region connection String SqlServer
            services.AddScoped<IDataBaseContext, DataBaseContext>();
            string connection = Configuration["ConnectionString:SqlServer"];
            services.AddDbContext<DataBaseContext>(option => option.UseSqlServer(connection));

            services.AddTransient<IIdentityDatabaseContext, IdentityDatabaseContext>();
            services.AddIdentityService(Configuration);
            services.AddAuthorization();
            services.ConfigureApplicationCookie(option =>
            {
                option.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                option.LoginPath = "/account/login";
                option.AccessDeniedPath = "/Account/AccessDenied";
                option.SlidingExpiration = true;
            });
            #endregion

            //mapper
            services.AddAutoMapper(typeof(DescountMapperProfile));
            services.AddAutoMapper(typeof(CatalogMappingProfile));
            services.AddAutoMapper(typeof(CatalogVMMappingProfile));


            //fluentValidation
            services.AddTransient<IValidator<AddNewCatalogItemDto>, AddNewCatalogItemDtoValidator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Seed data on application startup
            //using (var serviceScope = app.ApplicationServices.CreateScope())
            //{
            //    var dbContext = serviceScope.ServiceProvider.GetRequiredService<DataBaseContext>();
            //    var identityDatabase = serviceScope.ServiceProvider.GetRequiredService<IdentityDatabaseContext>();
            //    dbContext.Database.Migrate();
            //    identityDatabase.Database.Migrate();
            //    new ApplicationDbContextSeeder().SeedAsync(dbContext, identityDatabase, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            //}
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
         
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
          name: "areas",
           pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


                endpoints.MapControllerRoute(
                              name: "default",
                              pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        
        }
    }
}
