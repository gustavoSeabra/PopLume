using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using PopLume.Domain.Entities;
using PopLume.Infrastructure.DataProvider.EntityConfigurations;
using Xunit;

namespace PopLume.Tests.Unitarios.EntityConfigurations;

public class EquipamentoEntityConfigurationTests
{
    [Fact(DisplayName = "Deve configurar o valor por hora como coluna calculada persistida.")]
    public void Configure_DeveConfigurarValorHoraComoColunaCalculadaPersistida()
    {
        // Arrange
        var modelBuilder = new ModelBuilder(new ConventionSet());
        var entityBuilder = modelBuilder.Entity<Equipamento>();
        var configuration = new EquipamentoEntityConfiguration();

        // Act
        configuration.Configure(entityBuilder);

        // Assert
        var propriedade = entityBuilder.Metadata.FindProperty(nameof(Equipamento.ValorHora));

        propriedade.Should().NotBeNull();
        propriedade!.FindAnnotation("Relational:ComputedColumnSql")!.Value.Should().Be(
            """CASE WHEN "ExpectativaVida" > 0 THEN "ValorCompra" / "ExpectativaVida" ELSE 0 END""");
        propriedade.FindAnnotation("Relational:IsStored")!.Value.Should().Be(true);
    }
}
