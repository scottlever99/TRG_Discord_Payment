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
            builder.Services.AddScoped<DiscordService>();
            builder.Services.AddScoped<EmailService>();



            //TEst Stripe = "sk_test_51MygchAlSoLxBcHKjc6T16QCWcuHrZTOeJ2l67Fr4EbdbSLpwtzEXTpV6mAp0DkMnQV6G8o2SbVjZYF3VwtgIVFI00yjhgK614",
            //TEst Live Stripe = sk_test_51I88jPHy10dRIZQuiWpZ4xP3CSkYB6iOCiAzD9zXK4MJUbGHmeZiHGo0Qzz8UApg96GYWZi4lomzG1oaiCL8jZbI00m6HZrrTY 
            //Prod = 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
            var app = builder.Build();
            if (!app.Environment.IsDevelopment())
            {
                app.UseHsts();
            }
            app.UseExceptionHandler("/Home/Error");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
            app.Run();
        }
    }
}