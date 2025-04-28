using Microsoft.EntityFrameworkCore;
using MPService.Application.Auth;
using MPService.Application.Users;
using MPService.Domain.Users;
using MPService.Infrastructure.Persistence;
using MPService.Infrastructure.Persistence.Users;
using Scalar.AspNetCore;
using System;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy =>
        {
            policy.WithOrigins(
                "http://localhost:8081",
                "http://192.168.100.27:5000",
                "http://192.168.100.27:8082")
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

builder.WebHost.UseUrls(
    "http://0.0.0.0:5000",
    "http://0.0.0.0:8082");


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

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated(); // creates DB if it doesn't exist
}

app.Run();
