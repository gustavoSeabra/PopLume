using PopDesign.Application.Dtos;
using PopDesign.Domain.Entities;

namespace PopDesign.Application.Mappers;

public static class MarketplaceMapper
{
    public static MarketplaceDto ToDto(this Marketplace marketplace)
    {
        if (marketplace == null) return null!;

        return new MarketplaceDto
        {
            IdMarketplace = marketplace.IdMarketplace,
            Nome = marketplace.Nome,
            LinkLoja = marketplace.LinkLoja,
            Excluido = marketplace.Excluido,
            DataExclusao = marketplace.DataExclusao,
            TaxasMarketplace = marketplace.TaxasMarketplace?.Select(t => t.ToDto()).ToList()
        };
    }

    public static Marketplace ToEntity(this CreateMarketplaceDto dto)
    {
        if (dto == null) return null!;

        return new Marketplace
        {
            Nome = dto.Nome,
            LinkLoja = dto.LinkLoja,
            TaxasMarketplace = dto.TaxasMarketplace?
                .Select(t => t.ToEntity())
                .ToList() ?? new List<TaxasMarketplace>()
        };
    }
}
