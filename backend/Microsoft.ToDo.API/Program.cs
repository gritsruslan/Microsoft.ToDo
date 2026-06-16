using Microsoft.AspNetCore.Identity;
using Microsoft.ToDo.API.Middlewares;
using Microsoft.ToDo.Application;
using Microsoft.ToDo.Domain.Constants;
using Microsoft.ToDo.Domain.Models;
using Microsoft.ToDo.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

services
    .AddCors()
    .AddControllers();

services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ToDoDbContext>()
    .AddDefaultTokenProviders();

services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredLength = SecurityConstants.PasswordMinLength;
});

services
    .AddApplication()
    .AddAuth(configuration)
    .AddInfrastructure(configuration);

var app = builder.Build();

app.UseCors("Frontend");

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();

app.Run();