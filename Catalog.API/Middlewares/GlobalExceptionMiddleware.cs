using Microsoft.Data.SqlClient;

namespace Catalog.API.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (DbUpdateException)
        {
            await Results.Problem(
                    title: "Ocurrio un error al actualizar el registro en la base de datos",
                    statusCode: StatusCodes.Status500InternalServerError)
                .ExecuteAsync(httpContext);
        }
        catch (SqlException)
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