namespace Catalog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;    
    private readonly IHttpContextAccessor _httpContextAccessor;

    private string endpointSecret = string.Empty;

    public OrderController(IHttpContextAccessor httpContextAccessor,
        IHostEnvironment hostEnvironment,
        IOrderService orderService)
    {
        _orderService = orderService;        
        _httpContextAccessor = httpContextAccessor;

        if (hostEnvironment.IsProduction())
        {
            endpointSecret = "whsec_TGjZm10IX1UHQLZshNy2Ae2XPMFyPukU";
        }
        if (hostEnvironment.IsDevelopment())
        {
            endpointSecret = "whsec_2bacb0ffe54bdc9d94dd067b03cb0730c67ab567f143c5b76c5f8502ab5940b8";
        }
    }

    [HttpPost]
    public async Task<IResult> Post()
    {
        try
        {             
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeEvent = EventUtility.ParseEvent(json, throwOnApiVersionMismatch: false);

            if (stripeEvent.Type == Events.CheckoutSessionCompleted)
            {
                var session = stripeEvent.Data.Object as Session;
                if (session is not null)
                {
                    await _orderService.CreateAsync(session);
                }
            }
            return Results.Ok();
        }
        catch (Exception exeption)
        {
            return Results.BadRequest(exeption.Message);
        }
    }

    [HttpGet]
    [Authorize]
    public async Task<IResult> Get()
    {
        var id = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var orders = await _orderService.GetAsync(id);
        return Results.Ok(orders);
    }
}