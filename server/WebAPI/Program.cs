using System.Text;
using Application;
using Application.Interfaces;
using Domain.Enums;
using Hangfire;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WebAPI.Authentication;
using WebAPI.Hubs;
using WebAPI.Middlewares;
using WebAPI.Options;
using WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ReservationOptions>(
    builder.Configuration.GetSection(ReservationOptions.SectionName));

builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection(JwtOptions.SectionName));

var corsOptions = builder
    .Configuration
    .GetSection(CorsOptions.SectionName)
    .Get<CorsOptions>();

var jwtOptions = builder
    .Configuration
    .GetSection(JwtOptions.SectionName)
    .Get<JwtOptions>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsOptions.Name,
        policy =>
        {
            policy.WithOrigins(corsOptions.Origins)
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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
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
builder.Services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseInMemoryStorage()
);

builder.Services.AddHangfireServer();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddSingleton<ProductViewsService>();
builder.Services.AddScoped<IRealTimeService, RealTimeService>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(corsOptions.Name);
app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.UseHangfireDashboard();

app.MapHub<ProductHub>("/products");

app.Run();