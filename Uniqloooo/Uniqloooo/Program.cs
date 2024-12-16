using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Uniqloooo.Context;
using Uniqloooo.Extensions;
using Uniqloooo.Models;

namespace Uniqloooo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<UniqloDb>(opt=>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("MSsql"));
                
            } 

            );

            builder.Services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequireUppercase = true;

                opt.Password.RequireLowercase = true;
                opt.Password.RequiredLength = 8;
                opt.Password.RequireDigit = true;
                opt.Lockout.MaxFailedAccessAttempts = 1;

                opt.Password.RequireLowercase =true;
                opt.Password.RequiredLength = 8;
                opt.Password.RequireDigit = true;
                opt.Lockout.MaxFailedAccessAttempts=1;

                opt.Password.RequireNonAlphanumeric = true;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);

            }).AddDefaultTokenProviders().AddEntityFrameworkStores<UniqloDb>();
            builder.Services.ConfigureApplicationCookie(x =>
            {
                x.LoginPath = "/login";

                x.AccessDeniedPath = "/Home/AccessDenied";

            });
            //builder.Services.AddSession();
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
       
            app.UseUserSeed();  
            app.UseRouting();
            //app.UseSession();
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "login",
                pattern: "login" ,new
                {
                 Controller= "Account",
                 Action="Login"

                });
            app.MapControllerRoute(
               name: "register",
               pattern: "register", new
               {
                   Controller = "Account",
                   Action = "Register"

               });


            app.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=DashBoard}/{action=Index}/{id?}"
                );
            
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
           
            app.Run();
        }
    }
}
