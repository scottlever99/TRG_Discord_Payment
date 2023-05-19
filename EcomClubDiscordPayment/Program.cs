using EcomClubDiscordPayment.Data;
using EcomClubDiscordPayment.Services;
using Microsoft.EntityFrameworkCore;

namespace EcomClubDiscordPayment
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            string con = builder.Configuration.GetConnectionString("Default") ?? "";
            builder.Services.AddDbContext<DatabaseContext>(d => d.UseMySQL(con));

            builder.Services.AddScoped<DbService>();


            var app = builder.Build();
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
            app.Run();
        }
    }
}