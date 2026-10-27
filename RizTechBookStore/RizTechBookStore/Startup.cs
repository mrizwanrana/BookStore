using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RizTechBookStore.Data;
using RizTechBookStore.Helpers;
using RizTechBookStore.Models;
using RizTechBookStore.Repository;
using RizTechBookStore.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace RizTechBookStore
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BookStoreContext>(options =>
                options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<BookStoreContext>().AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 5;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;

                options.SignIn.RequireConfirmedEmail = true;
            });

            services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = _configuration["Application:LoginPath"];
            });

            services.AddControllersWithViews();
#if DEBUG
            services.AddRazorPages().AddRazorRuntimeCompilation();
            //services.AddRazorPages().AddRazorRuntimeCompilation().AddViewOptions(options =>
            //{
            //    options.HtmlHelperOptions.ClientValidationEnabled = false;
            //});
#endif
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<ILanguageRepository, LanguageRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<IMessageRepository, MessageRepository>();
            services.Configure<SMTPConfigModel>(_configuration.GetSection("SMTPConfig"));
            services.AddScoped<IEmailService, EmailService>();

            //services.Configure<NewBookAlertConfig>(_configuration.GetSection("NewBookAlert"));
            //services.Configure<NewBookAlertConfig>(_configuration.GetSection("SecondNewBookAlert"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //Below code is for middleware configuration in the request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
                //endpoints.MapControllerRoute(
                //    name: "default",
                //    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "MyArea",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            });

            //app.Use(async (context, next) =>
            //{
            //    await context.Response.WriteAsync("Hellow from my first middleware");

            //    await next();

            //    await context.Response.WriteAsync("Hello from my first middleware Response");
            //});

            //app.Use(async (context, next) =>
            //{
            //    await context.Response.WriteAsync("Hello from my second middleware");

            //    await next();

            //    await context.Response.WriteAsync("Hello from my second middleware Response");

            //});

            //app.Use(async (context, next) =>
            //{
            //    await context.Response.WriteAsync("Hello from my third middleware");

            //});

            //app.UseRouting();

            //app.UseEndpoints(endpoints =>
            //{
                //endpoints.MapGet("/", async context =>
                //{
                //    await context.Response.WriteAsync("Hello from " + env.EnvironmentName);

                    //if (env.IsDevelopment())
                    //{
                    //    await context.Response.WriteAsync("Hello from " + env.EnvironmentName);
                    //}
                    //else if (env.IsProduction())
                    //{
                    //    await context.Response.WriteAsync("Hello from " + env.EnvironmentName);
                    //}
                    //else if (env.IsStaging())
                    //{
                    //    await context.Response.WriteAsync("Hello from " + env.EnvironmentName);
                    //}
                    //else
                    //{
                    //    await context.Response.WriteAsync("Hello from " + env.EnvironmentName);
                    //}
                    //});
                //});
            //});
        }
    }
}
