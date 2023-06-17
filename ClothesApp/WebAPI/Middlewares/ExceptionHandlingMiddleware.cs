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
            context.Response.ContentType = "application/json";
            
            ProblemDetails problem = new()
            {
                Status = (int)HttpStatusCode.NotFound,
                Detail = e.Message,
            };

            string json = JsonSerializer.Serialize(problem);
            
            await context.Response.WriteAsync(json);
        }
        catch (BusinessRuleException e)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";

            ProblemDetails problem = new()
            {
                Status = (int)HttpStatusCode.BadRequest,
                Detail = e.Message,
            };
            
            string json = JsonSerializer.Serialize(problem);
            
            await context.Response.WriteAsync(json);
        }
    }
}