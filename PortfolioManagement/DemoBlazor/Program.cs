using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using DemoBlazor;
using MudBlazor.Services;
using Microsoft.Extensions.DependencyInjection;  // <-- Добавьте эту строку

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();

// Регистрация HttpClient и валидатора
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<SolanaAddressValidator>();

await builder.Build().RunAsync();
