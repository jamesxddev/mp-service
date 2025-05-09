using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MPService.API.Models;
using MPService.Application.Auth;
using MPService.Application.Shifts;
using MPService.Application.Users;
using MPService.Domain.Shifts;
using MPService.Domain.Users;
using MPService.Infrastructure.Persistence;
using MPService.Infrastructure.Persistence.Shifts;
using MPService.Infrastructure.Persistence.Users;
using Scalar.AspNetCore;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy =>
        {
            policy.WithOrigins(
                "http://localhost:8081",
                "http://192.168.100.27:5000",
                "http://192.168.100.27:8082",
                "http://192.168.1.100:5000",
                "http://192.168.1.100:8082")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddApplicationDbContext();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserAppService, UserAppService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IShiftRepository, ShiftRepository>();
builder.Services.AddScoped<IShiftAppService, ShiftAppService>();

builder.WebHost.UseUrls(
    "http://0.0.0.0:5000",
    "http://0.0.0.0:8081",
    "http://0.0.0.0:8082");

// Bind config to class
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();

if (jwtSettings == null || string.IsNullOrWhiteSpace(jwtSettings.Key) || string.IsNullOrWhiteSpace(jwtSettings.Issuer))
{
    throw new InvalidOperationException("JWT configuration is missing or incomplete in appsettings.json.");
}

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings!.Issuer,
            ValidAudience = jwtSettings!.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings!.Key))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.MapGet("/", context =>
    {
        context.Response.Redirect("/scalar/v1");
        return Task.CompletedTask;
    });
}

app.MapGet("/info", () =>
{
    var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "Unknown";
    return $"MP Service App Version: {version}";

});

app.UseHttpsRedirection();
app.UseCors("AllowLocalhost");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated(); // creates DB if it doesn't exist
}

app.Run();
