using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using WebDiaryAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorApp", policy =>
        policy.WithOrigins("https://localhost:7055") // Replace with your Blazor app's URL.
              .AllowAnyHeader()
              .AllowAnyMethod());
});

builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();
    //app.MapScalarApiReference(options =>
    //{
    //    options.Theme = ScalarTheme.BluePlanet;
    //});
}

app.UseHttpsRedirection();

app.UseCors("AllowBlazorApp");

app.UseAuthorization();

app.MapControllers();

app.Run();