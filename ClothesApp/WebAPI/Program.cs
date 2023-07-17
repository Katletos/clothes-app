using System.Text;
using Application;
using Domain.Enums;
using Infrastructure;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WebAPI.Authentication;
using WebAPI.Cors;
using WebAPI.Hubs;
using WebAPI.Middlewares;
using WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(CustomCorsConfiguration.Name,
        policy =>
        {
            policy.WithOrigins(CustomCorsConfiguration.Origins)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])),
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policies.Admin, policy =>
        policy.RequireClaim(CustomClaims.UserType, UserType.Admin.ToString()));
    options.AddPolicy(Policies.Customer, policy =>
        policy.RequireClaim(CustomClaims.UserType, UserType.Customer.ToString()));
});
builder.Services.AddSignalR();
// builder.Services.AddHangfire( config => config
//     .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
//     .UseSimpleAssemblyNameTypeSerializer()
//     .UseRecommendedSerializerSettings()
//     .UsePostgreSqlStorage(builder.Configuration.GetConnectionString("DefaultConnection"),
//         new PostgreSqlStorageOptions()
//     {
//         QueuePollInterval = TimeSpan.FromSeconds(5),
//         SchemaName = "schema",
//     })
// );
// builder.Services.AddHangfireServer();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddSingleton<ProductViews>();
builder.Services.AddScoped<ISignalRService, SignalRService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(CustomCorsConfiguration.Name);
app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// app.UseHangfireDashboard();
// app.MapHangfireDashboard();

app.MapHub<ProductHub>("/products");

app.Run();