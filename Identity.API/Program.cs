using Identity.API.Data;
using Identity.API.Models;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(option =>
{
	option.AddPolicy("default", config =>
	{
		config.AllowAnyMethod();
		config.AllowAnyHeader();
		config.AllowAnyOrigin();
	});
});

builder.Services.AddDefaultIdentity<UserStore>(option =>
	option.SignIn.RequireConfirmedEmail = true)
	.AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddIdentityServer()
	.AddApiAuthorization<UserStore, AppDbContext>();

builder.Services.AddAuthentication()
	.AddIdentityServerJwt();

builder.Services.Configure<JwtBearerOptions>(
	IdentityServerJwtConstants.IdentityServerJwtBearerScheme, options =>
	{
		options.TokenValidationParameters.ValidateActor = false;
	});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseIdentityServer();
app.UseAuthentication();
app.UseEndpoints(endpoints =>
{
	endpoints.MapRazorPages();
	endpoints.MapControllers();
});

var summaries = new[]
{
	"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
	var forecast = Enumerable.Range(1, 5).Select(index =>
		new WeatherForecast
		(
			DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
			Random.Shared.Next(-20, 55),
			summaries[Random.Shared.Next(summaries.Length)]
		))
		.ToArray();
	return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
	public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
