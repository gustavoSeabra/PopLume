using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using PopLume.Domain.Entities;
using PopLume.Infrastructure.DataProvider.EntityConfigurations;
using Xunit;

namespace PopLume.Tests.Unitarios.EntityConfigurations;

public class FilamentoEntityConfigurationTests
{
    [Fact(DisplayName = "Deve persistir o tipo do filamento como texto.")]
    public void Configure_DeveConfigurarConversaoDoTipoParaTexto()
    {
        var modelBuilder = new ModelBuilder(new ConventionSet());
        var entityBuilder = modelBuilder.Entity<Filamento>();

        new FilamentoEntityConfiguration().Configure(entityBuilder);

        var propriedade = entityBuilder.Metadata.FindProperty(nameof(Filamento.Tipo));
        propriedade.Should().NotBeNull();
        propriedade!.GetValueConverter().Should().NotBeNull();
        propriedade.GetMaxLength().Should().Be(10);
    }
}
