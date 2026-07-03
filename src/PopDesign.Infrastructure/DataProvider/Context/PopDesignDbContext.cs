﻿using Microsoft.EntityFrameworkCore;
using PopDesign.Domain.Entities;
using PopDesign.Domain.Repositories;

namespace PopDesign.Infrastructure.DataProvider.Context;

public class PopDesignDbContext: DbContext, IUnitOfWork
{
    public PopDesignDbContext(DbContextOptions<PopDesignDbContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PopDesignDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ChangeTracker.DetectChanges();
        return base.SaveChangesAsync(cancellationToken);
    }

    public DbSet<Equipamento> Equipamentos { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<ProdutoComposicao> ProdutoComposicoes { get; set; }
    public DbSet<Marketplace> Marketplaces { get; set; }
    public DbSet<TaxasMarketplace> TaxasMarketplace { get; set; }
}
