using Microsoft.EntityFrameworkCore;
using MPService.Application.Users;
using MPService.Domain.Users;
using MPService.Infrastructure.Persistence;
using MPService.Infrastructure.Persistence.Users;
using Scalar.AspNetCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddApplicationDbContext();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserAppService, UserAppService>();


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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
