﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PopLume.Domain.Repositories;
using PopLume.Application.Services.Interfaces;
using PopLume.Application.Services;
using PopLume.Infrastructure.Repositories;

namespace PopLume.Infrastructure;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApiDependencyGroup(this IServiceCollection services, IConfiguration configuration)
    {
        // Services
        services.AddScoped<IProdutoService, ProdutoService>();
        services.AddScoped<IEquipamentoService, EquipamentoService>();
        services.AddScoped<IMarketplaceService, MarketplaceService>();

        // Repositórios
        services.AddScoped<IEquipamentoRepository, EquipamentoRepository>();
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<IMarketplaceRepository, MarketplaceRepository>();

        return services;
    }
}
