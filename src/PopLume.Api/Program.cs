using PopLume.Api.Extensions;
using PopLume.Api.Middlewares;
using PopLume.Infrastructure;
using PopLume.Infrastructure.Extensions;
using Scalar.AspNetCore;
using Serilog;
using System.Globalization;


var culture = CultureInfo.GetCultureInfo("pt-BR");
Thread.CurrentThread.CurrentCulture = culture;
Thread.CurrentThread.CurrentUICulture = culture;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Adicionando as dependencias do projeto
builder.Services
    .AddDatabase(builder.Configuration)
    .AddApiDependencyGroup(builder.Configuration)
    .AddValidationConfiguration();

// Adicionando suporte para log usando o SERILOG
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.MapOpenApi();
    app.MapScalarApiReference();

    app.MapGet("/", () => Results.Redirect("/scalar")).ExcludeFromDescription();
//}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
