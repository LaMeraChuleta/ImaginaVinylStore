using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (DbUpdateException ex)
        {
            await Results.Problem(
                    title: "Ocurrio un error al actualizar el registro en la base de datos",
                    statusCode: StatusCodes.Status500InternalServerError)
                .ExecuteAsync(httpContext);
        }
        catch (SqlException ex)
        {
            await Results.Problem(
                    title: "Ocurrio un error en la base de datos",
                    statusCode: StatusCodes.Status500InternalServerError)
                .ExecuteAsync(httpContext);
        }
        catch (Exception ex)
        {
            await Results.Problem(
                    title: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError)
                .ExecuteAsync(httpContext);
        }
    }
}