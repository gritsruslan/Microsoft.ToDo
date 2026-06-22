using Microsoft.AspNetCore.Identity;
using Microsoft.ToDo.API.Extensions;
using Microsoft.ToDo.API.Middlewares;
using Microsoft.ToDo.Application;
using Microsoft.ToDo.Domain.Models;
using Microsoft.ToDo.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
var environment = builder.Environment;

services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

if (environment.IsDevelopment())
{
    var frontendOptions = configuration
        .GetRequiredSection(nameof(FrontendOptions))
        .Get<FrontendOptions>()!;
    
    services.AddCorsPolicy(frontendOptions);
}

services.AddControllers();

services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ToDoDbContext>()
    .AddDefaultTokenProviders();

services.ConfigureIdentityOptions();

services
    .AddApplication()
    .AddAuth(configuration)
    .AddInfrastructure(configuration);

var app = builder.Build();

if (environment.IsDevelopment())
{
    app.UseCors(CorsExtensions.FrontendPolicyName);
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();

app.Run();