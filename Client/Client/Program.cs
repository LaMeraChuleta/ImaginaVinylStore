using Client.App;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("Client.ServerAPI", client =>
{
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
});

builder.Services.AddHttpClient("CatalogMusic.API", client =>
{
    client.BaseAddress = new Uri(@"https://catalogimaginaapi.azurewebsites.net/api/");
});

builder.Services.AddApiAuthorization();
await builder.Build().RunAsync();