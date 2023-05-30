using System.Net;
using System.Text.Json;
using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Middlewares;

public class ExceptionHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (NotFoundException e)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;

            ProblemDetails problem = new()
            {
                Status = (int)HttpStatusCode.NotFound,
                Detail = e.Message,
            };
            
            string json = JsonSerializer.Serialize(problem);
            
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(json);
        }
        catch (DatabaseConflictException e)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Conflict;
            
            ProblemDetails problem = new()
            {
                Status = (int)HttpStatusCode.Conflict,
                Detail = e.Message,
            };
            
            string json = JsonSerializer.Serialize(problem);
            
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(json);
        }
    }
}