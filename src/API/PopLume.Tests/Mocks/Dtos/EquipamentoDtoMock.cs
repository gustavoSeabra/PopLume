using Bogus;
using PopLume.Application.Dtos;
using PopLume.Domain.Entities;

namespace PopLume.Tests.Mocks.Dtos;

public static class EquipamentoDtoMock
{
    private static readonly Faker Faker = new("pt_BR");

    public static CreateEquipamentoDto CreateEquipamentoDtoValido() =>
        new()
        {
            Nome = Faker.Commerce.ProductName(),
            Apelido = Faker.Commerce.ProductAdjective(),
            DataCompra = DateOnly.FromDateTime(Faker.Date.Past()),
            Potencia = Faker.Random.Int(100, 1000),
            ValorCompra = Faker.Finance.Amount(500, 10000),
            ExpectativaVida = Faker.Random.Int(1000, 10000)
        };

    public static UpdateEquipamentoDto UpdateEquipamentoDtoValido(Guid? idEquipamento = null) =>
        new()
        {
            IdEquipamento = idEquipamento ?? Guid.NewGuid(),
            Nome = Faker.Commerce.ProductName(),
            Apelido = Faker.Commerce.ProductAdjective(),
            DataCompra = DateOnly.FromDateTime(Faker.Date.Past()),
            Potencia = Faker.Random.Int(100, 1000),
            ValorCompra = Faker.Finance.Amount(500, 10000),
            ExpectativaVida = Faker.Random.Int(1000, 10000)
        };

    public static Equipamento EquipamentoValido(Guid? idEquipamento = null, string? nome = null, string? apelido = null) =>
        new()
        {
            IdEquipamento = idEquipamento ?? Guid.NewGuid(),
            Nome = nome ?? Faker.Commerce.ProductName(),
            Apelido = apelido ?? Faker.Commerce.ProductAdjective(),
            DataCompra = DateOnly.FromDateTime(Faker.Date.Past()),
            Potencia = Faker.Random.Int(100, 1000),
            ValorCompra = Faker.Finance.Amount(500, 10000),
            ExpectativaVida = Faker.Random.Int(1000, 10000)
        };

    public static Equipamento EquipamentoDesativado(Guid? idEquipamento = null)
    {
        var equipamento = EquipamentoValido(idEquipamento);
        equipamento.Excluir();

        return equipamento;
    }

    public static List<Equipamento> EquipamentosValidos(int quantidade = 3) =>
        Enumerable.Range(0, quantidade)
            .Select(_ => EquipamentoValido())
            .ToList();

    public static List<Equipamento> EquipamentosDesativados(int quantidade = 3) =>
        Enumerable.Range(0, quantidade)
            .Select(_ => EquipamentoDesativado())
            .ToList();
}
