
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(b => b.MigrationsAssembly("Catalog.API")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

StripeConfiguration.ApiKey = "sk_test_51NplEDF4FZD9gDsPR6aXDamqBbOPU0Pr7UIQaJoPyXlbSwzujJ0e7G4X1DWvK8Re6WZ8R61G25e9R2YHaGj8Ef8M00EyzWPhqp";

var urlServer = string.Empty;
if (builder.Environment.IsDevelopment())
{
    urlServer = "https://localhost:7197/"; 
}
if (builder.Environment.IsProduction())
{
    urlServer = "https://imaginavinyl.azurewebsites.net/";
}
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = urlServer;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IURLStripeService, URLStripeService>();
builder.Services.AddScoped<IProductStripeService, ProductStripeService>();

var app = builder.Build();
app.UseCors();
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();