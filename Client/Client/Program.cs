using Blazored.Toast;
using Client.App;
using Client.App.Shared;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("Client.ServerAPI",
    client => { client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); });

builder.Services.AddHttpClient("CatalogMusic.API",
    client => { client.BaseAddress = new Uri(@"https://localhost:7285/api/"); });

builder.Services.AddBlazoredToast();
builder.Services.AddApiAuthorization();

await builder.Build().RunAsync();