using Application;
using Infrastructure;
using Infrastructure.Authentication;
using WebAPI.Authentication;
using WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

var myAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(myAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://127.0.0.1:5500", "http://127.0.0.1:5500/#")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policies.Admin, policy =>
        policy.RequireClaim(CustomClaims.Role, Roles.Admin));
    options.AddPolicy(Policies.Customer, policy =>
        policy.RequireClaim(CustomClaims.Role, Roles.Customer));
});

builder.Services.AddAuthorization();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(myAllowSpecificOrigins);
app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();