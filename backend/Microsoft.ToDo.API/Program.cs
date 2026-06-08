using Microsoft.AspNetCore.Identity;
using Microsoft.ToDo.API.Middlewares;
using Microsoft.ToDo.Application;
using Microsoft.ToDo.Domain.Models;
using Microsoft.ToDo.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

services.AddControllers();

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ToDoDbContext>()
    .AddDefaultTokenProviders();

services
    .AddApplication()
    .AddAuth(configuration)
    .AddInfrastructure(configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();

app.Run();