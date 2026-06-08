using Microsoft.ToDo.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

services.AddInfrastructure(configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();