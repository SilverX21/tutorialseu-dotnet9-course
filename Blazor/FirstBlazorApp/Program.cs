using FirstBlazorApp;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp =>
new HttpClient
{
    //BaseAddress = new Uri("https://localhost:7203/WeatherForecast?location=Braga")
});

await builder.Build().RunAsync();