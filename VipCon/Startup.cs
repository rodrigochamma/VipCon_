using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VipCon.Data;
using VipCon.Services;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using AutoMapper;
using VipCon.Helpers;

namespace VipCon
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, MyIdentityRole> (options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })            
                .AddErrorDescriber<TraducaoIdentityErrorDescriber>()                
                .AddEntityFrameworkStores<ApplicationDbContext>()                
                .AddDefaultTokenProviders();

            services.AddAutoMapper();
            services.Configure<ConfiguracoesSMTP>(Configuration.GetSection("ConfiguracoesSMTP"));

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            services.AddMvc()
                .AddRazorPagesOptions(options =>
                {                    
                    options.Conventions.AuthorizePage("/Account/Logout");
                    options.Conventions.AuthorizePage("/LinksUteis");
                    options.Conventions.AuthorizeFolder("/Account/Manage");
                    options.Conventions.AuthorizeFolder("/Usuarios");
                    options.Conventions.AuthorizeFolder("/Noticias");
                    options.Conventions.AuthorizeFolder("/Parceiros");
                    options.Conventions.AuthorizeFolder("/Prospect");
                    options.Conventions.AllowAnonymousToPage("/Parceiros/Listagem");
                });

            

            // Register no-op EmailSender used by account confirmation and password reset during development
            // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=532713
            services.AddSingleton<IEmailSender, EmailSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, UserManager<ApplicationUser> userManager,RoleManager<MyIdentityRole> roleManager)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
                       

            app.UseStaticFiles();

            
            app.UseAuthentication();

            MyIdentityDataInitializer.SeedData(userManager, roleManager);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}
