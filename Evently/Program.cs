using BusinessLogic.IServices;
using BusinessLogic.Services;
using DataAccess.DBContext;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Models.IRepositories;
using Microsoft.AspNetCore.Identity;
using Models.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Utilities;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.Extensions.Options;

namespace Evently
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            // Add services to the container.
            builder.Services.AddControllersWithViews()
                .AddViewLocalization() // Enables view localization
                .AddDataAnnotationsLocalization(); // Enables localization for model validation messages


            // Configure supported cultures (English and Arabic)
            var supportedCultures = new[]
            {
                new CultureInfo("en"),
                new CultureInfo("ar")
            };

            // Configure Request Localization options
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("ar"); // Default language
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                // Use a cookie provider to persist user culture preference
                options.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());
            });

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(4))
                .AddDefaultTokenProviders().AddDefaultUI()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddSingleton<IEmailSender, EmailSender>();

            // register repositories
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IEventRepository, EventRepository>();
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();

            // register services
            builder.Services.AddScoped<IServicesProvider, ServicesProvider>();
            builder.Services.AddScoped<IEventService, EventService>();
            builder.Services.AddScoped<IBookingService, BookingService>();

            // Add localization services
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
            builder.Services.AddMvc()
                .AddViewLocalization()
                .AddDataAnnotationsLocalization();

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("ar")
                };

                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication(); // add this line to enable authentication
            // Apply Request Localization Middleware (IMPORTANT)
            var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
            app.UseRequestLocalization(localizationOptions);
            app.UseAuthorization();
            app.MapRazorPages(); // because the Identity UI is Razor Pages

            
            app.MapControllerRoute(
                name: "User",
                pattern: "{area=User}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Admin}/{controller=Event}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
