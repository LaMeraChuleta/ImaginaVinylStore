using Blazored.LocalStorage;
using Blazored.Toast;
using Client.App;
using Client.App.Interfaces;
using Client.App.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("Client.ServerAPI",
    client => { client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress + "api/"); });

builder.Services.AddHttpClient("CatalogMusic.API",
        client => { client.BaseAddress = new Uri(@"https://localhost:7285/api/"); });    

builder.Services.AddBlazoredToast();
builder.Services.AddBlazoredLocalStorageAsSingleton();
builder.Services.AddApiAuthorization();

builder.Services.AddTransient<IHttpClientHelperService, HttpClientHelperService>();
builder.Services.AddScoped<IShopCartService, ShopCartService>();
builder.Services.AddScoped<ICatalogMusicService, CatalogMusicService>();
builder.Services.AddScoped<IAudioCatalogService, AudioCatalogService>();
builder.Services.AddScoped<IArtistService, ArtistService>();
builder.Services.AddScoped<IFormatService, FormatService>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IPresentationService, PresentationService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IShopCartNotificationService, ShopCartNotificationService>();
builder.Services.AddScoped<IOrderService, OrderService>();

await builder.Build().RunAsync();