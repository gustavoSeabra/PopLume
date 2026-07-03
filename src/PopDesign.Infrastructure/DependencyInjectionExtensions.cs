﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PopDesign.Domain.Repositories;
using PopDesign.Application.Services.Interfaces;
using PopDesign.Application.Services;
using PopDesign.Infrastructure.Repositories;

namespace PopDesign.Infrastructure;

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
