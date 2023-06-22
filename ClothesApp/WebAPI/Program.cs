using System.Text;
using Application;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// //builder.Configuration.AddJsonFile();        var configuration = new ConfigurationBuilder()
//                                                   .SetBasePath(Directory.GetCurrentDirectory())
//                                                   .AddJsonFile("appsettings.json")
//                                                   .Build();
//builder.Configuration.AddJsonFile("appsettings.json");
// builder.Services.ConfigureOptions<JwtOptionsSetup>();
// builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();
// builder.Services.Configure<JwtOptionsSetup>(builder.Configuration);
//builder.Services.Configure<JwtBearerOptionsSetup>(builder.Configuration);

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(myAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://127.0.0.1:5500", "http://127.0.0.1:5500/#").AllowAnyHeader().AllowAnyMethod()
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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireClaim("Role", "Admin"));
    options.AddPolicy("Customer", policy => policy.RequireClaim("Role", "Customer"));
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