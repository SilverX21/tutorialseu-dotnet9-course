using DevSpot.Data;
using DevSpot.Models;
using DevSpot.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DevSpot;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("Database"));
            options.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        });

        //IdentityUser is the default user we see in the database table, it's the default :)
        builder.Services.AddDefaultIdentity<IdentityUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
        })
        .AddRoles<IdentityRole>() //this is adding the default roles
        .AddEntityFrameworkStores<ApplicationDbContext>(); //if we don't do this we can't create users or roles

        //DI
        builder.Services.AddScoped<IRepository<JobPosting>, JobPostingRepository>();

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            //since the method is static, we can use the .Wait() to await for the response
            RoleSeeder.SeedRolesAsync(services).Wait();
            UserSeeder.SeedUsersAsync(services).Wait();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthorization();

        //here we are adding endpoints to the razor pages. Without this we cannot navigate to the pages
        app.MapRazorPages();

        app.MapStaticAssets();

        //here we are adding endpoints to the controllers. Without this we cannot navigate to the pages
        //we are pointing to the job postings controller as the home controller
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=JobPostings}/{action=Index}/{id?}")
            .WithStaticAssets();

        app.Run();
    }
}