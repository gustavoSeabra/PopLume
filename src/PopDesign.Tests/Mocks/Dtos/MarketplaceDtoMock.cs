using Bogus;
using PopDesign.Application.Dtos;
using PopDesign.Domain.Entities;

namespace PopDesign.Tests.Mocks.Dtos;

public static class MarketplaceDtoMock
{
    private static readonly Faker Faker = new("pt_BR");

    public static CreateMarketplaceDto CreateMarketplaceDtoValido(int quantidadeTaxas = 2) =>
        new()
        {
            Nome = Faker.Company.CompanyName(),
            LinkLoja = Faker.Internet.Url(),
            TaxasMarketplace = Enumerable.Range(0, quantidadeTaxas)
                .Select(_ => CreateMarketplaceTaxaDtoValida())
                .ToList()
        };

    public static UpdateMarketplaceDto UpdateMarketplaceDtoValido(Guid? idMarketplace = null, List<UpdateMarketplaceTaxaDto>? taxas = null) =>
        new()
        {
            IdMarketplace = idMarketplace ?? Guid.NewGuid(),
            Nome = Faker.Company.CompanyName(),
            LinkLoja = Faker.Internet.Url(),
            TaxasMarketplace = taxas
        };

    public static CreateMarketplaceTaxaDto CreateMarketplaceTaxaDtoValida() =>
        new()
        {
            ValorInicial = Faker.Finance.Amount(0, 100),
            ValorFinal = Faker.Finance.Amount(101, 500),
            Comissao = Faker.Finance.Amount(1, 30, 2),
            TaxaFixa = Faker.Finance.Amount(0, 50)
        };

    public static UpdateMarketplaceTaxaDto UpdateMarketplaceTaxaDtoValida(Guid? idTaxa = null) =>
        new()
        {
            IdTaxa = idTaxa,
            ValorInicial = Faker.Finance.Amount(0, 100),
            ValorFinal = Faker.Finance.Amount(101, 500),
            Comissao = Faker.Finance.Amount(1, 30, 2),
            TaxaFixa = Faker.Finance.Amount(0, 50)
        };

    public static Marketplace MarketplaceValido(Guid? idMarketplace = null, string? nome = null, int quantidadeTaxas = 2)
    {
        var marketplace = new Marketplace
        {
            IdMarketplace = idMarketplace ?? Guid.NewGuid(),
            Nome = nome ?? Faker.Company.CompanyName(),
            LinkLoja = Faker.Internet.Url()
        };

        marketplace.TaxasMarketplace = Enumerable.Range(0, quantidadeTaxas)
            .Select(_ => TaxaMarketplaceValida(marketplace.IdMarketplace))
            .ToList();

        return marketplace;
    }

    public static TaxasMarketplace TaxaMarketplaceValida(Guid idMarketplace, Guid? idTaxa = null) =>
        new()
        {
            IdTaxa = idTaxa ?? Guid.NewGuid(),
            IdMarketplace = idMarketplace,
            ValorInicial = Faker.Finance.Amount(0, 100),
            ValorFinal = Faker.Finance.Amount(101, 500),
            Comissao = Faker.Finance.Amount(1, 30, 2),
            TaxaFixa = Faker.Finance.Amount(0, 50)
        };

    public static Marketplace MarketplaceDesativado(Guid? idMarketplace = null)
    {
        var marketplace = MarketplaceValido(idMarketplace);
        marketplace.Excluir();

        return marketplace;
    }

    public static List<Marketplace> MarketplacesValidos(int quantidade = 3) =>
        Enumerable.Range(0, quantidade)
            .Select(_ => MarketplaceValido())
            .ToList();

    public static List<Marketplace> MarketplacesDesativados(int quantidade = 3) =>
        Enumerable.Range(0, quantidade)
            .Select(_ => MarketplaceDesativado())
            .ToList();
}
