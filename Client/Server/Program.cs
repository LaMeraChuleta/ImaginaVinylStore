using Client.Server.Data;
using Client.Server.Models;
using Duende.IdentityServer.Models;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using SharedApp.Data;
using Client = Duende.IdentityServer.Models.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer("Server=localhost;Database=test;User Id=sa;Password=VacaLoca69;TrustServerCertificate=True;"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => { options.SignIn.RequireConfirmedAccount = true; })
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("Server=localhost;Database=test;User Id=sa;Password=VacaLoca69;TrustServerCertificate=True;"));


builder.Services.AddIdentityServer()
    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>(options =>
    {
        options.Clients.Add(new Duende.IdentityServer.Models.Client()
        {
            ClientId = "Client_App",
            ClientSecrets = { new Secret("Client_App".ToSha256()) },
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            AllowedScopes = { "Client.App" }
        });
    });


builder.Services.AddAuthentication()
    .AddIdentityServerJwt();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();