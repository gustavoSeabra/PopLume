﻿using Microsoft.EntityFrameworkCore;
using PopLume.Domain.Entities;
using PopLume.Domain.Repositories;

namespace PopLume.Infrastructure.DataProvider.Context;

public class PopLumeDbContext: DbContext, IUnitOfWork
{
    public PopLumeDbContext(DbContextOptions<PopLumeDbContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PopLumeDbContext).Assembly);

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
    public DbSet<Filamento> Filamentos { get; set; }
}
