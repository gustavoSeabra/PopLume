using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using PopLume.Api.Extensions;
using PopLume.Api.Middlewares;
using PopLume.Infrastructure;
using PopLume.Infrastructure.DataProvider.Context;
using PopLume.Infrastructure.Extensions;
using Scalar.AspNetCore;
using Serilog;
using System.Globalization;
using System.Text.Json.Serialization;


var culture = CultureInfo.GetCultureInfo("pt-BR");
Thread.CurrentThread.CurrentCulture = culture;
Thread.CurrentThread.CurrentUICulture = culture;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services
    .AddHealthChecks()
    .AddDbContextCheck<PopLumeDbContext>(
        name: "database",
        tags: ["ready"]);

// Adicionando as dependencias do projeto
builder.Services
    .AddDatabase(builder.Configuration)
    .AddApiDependencyGroup(builder.Configuration)
    .AddValidationConfiguration();

// Adicionando suporte para log usando o SERILOG
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = healthCheck => healthCheck.Tags.Contains("ready")
}).ExcludeFromDescription();

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
