using PopDesign.Application.Dtos;
using PopDesign.Domain.Entities;

namespace PopDesign.Application.Mappers;

public static class TaxasMarketplaceMapper
{
    public static TaxasMarketplaceDto ToDto(this TaxasMarketplace taxaMarketplace)
    {
        if (taxaMarketplace == null) return null!;

        return new TaxasMarketplaceDto
        {
            IdTaxa = taxaMarketplace.IdTaxa,
            IdMarketplace = taxaMarketplace.IdMarketplace,
            ValorInicial = taxaMarketplace.ValorInicial,
            ValorFinal = taxaMarketplace.ValorFinal,
            Comissao = taxaMarketplace.Comissao,
            TaxaFixa = taxaMarketplace.TaxaFixa,
            MarketplaceNome = taxaMarketplace.Marketplace?.Nome ?? string.Empty
        };
    }

    public static TaxasMarketplace ToEntity(this CreateMarketplaceTaxaDto dto)
    {
        if (dto == null) return null!;

        return new TaxasMarketplace
        {
            ValorInicial = dto.ValorInicial ?? 0m,
            ValorFinal = dto.ValorFinal ?? 0m,
            Comissao = dto.Comissao ?? 0m,
            TaxaFixa = dto.TaxaFixa ?? 0m
        };
    }
}
