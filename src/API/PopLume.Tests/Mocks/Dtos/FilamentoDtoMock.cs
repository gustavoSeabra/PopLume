using Bogus;
using PopLume.Application.Dtos;
using PopLume.Domain.Entities;
using PopLume.Domain.Enums;

namespace PopLume.Tests.Mocks.Dtos;

public static class FilamentoDtoMock
{
    private static readonly Faker Faker = new("pt_BR");

    public static CreateFilamentoDto CreateFilamentoDtoValido() =>
        new()
        {
            Cor = Faker.Commerce.Color(),
            Valor = Faker.Finance.Amount(50, 500),
            Peso = Faker.Random.Decimal(250, 5000),
            Tipo = Faker.PickRandom<TipoFilamento>(),
            DataCompra = DateOnly.FromDateTime(Faker.Date.Past())
        };

    public static UpdateFilamentoDto UpdateFilamentoDtoValido(Guid? idFilamento = null) =>
        new()
        {
            IdFilamento = idFilamento ?? Guid.NewGuid(),
            Cor = Faker.Commerce.Color(),
            Valor = Faker.Finance.Amount(50, 500),
            Peso = Faker.Random.Decimal(250, 5000),
            Tipo = Faker.PickRandom<TipoFilamento>(),
            DataCompra = DateOnly.FromDateTime(Faker.Date.Past())
        };

    public static Filamento FilamentoValido(Guid? idFilamento = null) =>
        new()
        {
            IdFilamento = idFilamento ?? Guid.NewGuid(),
            Cor = Faker.Commerce.Color(),
            Valor = Faker.Finance.Amount(50, 500),
            Peso = Faker.Random.Decimal(250, 5000),
            Tipo = Faker.PickRandom<TipoFilamento>(),
            DataCompra = DateOnly.FromDateTime(Faker.Date.Past())
        };
}
