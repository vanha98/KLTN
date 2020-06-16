using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Data.Bo;
using Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using KLTN.Authorization.Handlers;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace KLTN
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
            
            services.AddDbContext<KLTNContext>(options => options.UseLazyLoadingProxies().UseSqlServer(Configuration.GetConnectionString("KLTN")));
            services.AddIdentity<AppUser, AppRole>()
                    .AddRoles<AppRole>()
                    // services.AddDefaultIdentity<IdentityUser>()
                    .AddEntityFrameworkStores<KLTNContext>()
                    .AddDefaultTokenProviders();

            services.AddAutoMapper(typeof(Startup));
            
            services.AddScoped<UserManager<AppUser>, UserManager<AppUser>>();
            services.AddScoped<SignInManager<AppUser>, SignInManager<AppUser>>();
            services.AddScoped<IDeTaiNghienCuu, DeTaiNghienCuuBo>();
            services.AddScoped<ISinhVien, SinhVienBo>();
            services.AddScoped<INhom, NhomBo>();
            services.AddScoped<INhomSinhVien, NhomSinhVienBo>();
            services.AddScoped<IBaiPost, BaiPostBo>();
            services.AddScoped<IImgBaiPost, ImgBaiPostBo>();
            services.AddScoped<IKenhThaoLuan, KenhThaoLuanBo>();
            services.AddScoped<IIdentity, Identity>();

            services.AddControllersWithViews().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


            services.ConfigureApplicationCookie(options =>
            {
                //options.AccessDeniedPath = "/Account/AccessDenied";
                options.Cookie.Name = "Cookie";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(720);
                options.LoginPath = "/Home/Login";
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });

            services.AddScoped<IAuthorizationHandler, OwnerAuthorization>();
            services.AddScoped<IAuthorizationHandler, OwnerThaoLuanAuthorization>();
            //services.AddScoped<IRepository<DeTaiNghienCuu>, DangKyDeTaiBo>();
            

            //services.AddMvc(options => options.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "SinhVien",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "GVHD",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                //endpoints.MapControllerRoute(
                //    name: "Identity",
                //    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Login}/{id?}");

            });
            
            
        }
    }
}
