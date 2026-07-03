using FluentValidation;
using PopDesign.Application.Dtos;

namespace PopDesign.Application.Validators;

public class CreateMarketplaceValidator : AbstractValidator<CreateMarketplaceDto>
{
    public CreateMarketplaceValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome do marketplace é obrigatório.")
            .MaximumLength(100).WithMessage("O nome do marketplace deve ter no máximo 100 caracteres.");

        RuleFor(x => x.LinkLoja)
            .MaximumLength(100).WithMessage("O link da loja deve ter no máximo 100 caracteres.");

        RuleForEach(x => x.TaxasMarketplace)
            .SetValidator(new CreateMarketplaceTaxaValidator());
    }
}

public class UpdateMarketplaceValidator : AbstractValidator<UpdateMarketplaceDto>
{
    public UpdateMarketplaceValidator()
    {
        RuleFor(x => x.IdMarketplace)
            .NotEmpty().WithMessage("O ID do marketplace é obrigatório.");

        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome do marketplace é obrigatório.")
            .MaximumLength(100).WithMessage("O nome do marketplace deve ter no máximo 100 caracteres.");

        RuleFor(x => x.LinkLoja)
            .MaximumLength(100).WithMessage("O link da loja deve ter no máximo 100 caracteres.");

        RuleForEach(x => x.TaxasMarketplace)
            .SetValidator(new UpdateMarketplaceTaxaValidator());
    }
}

public class CreateMarketplaceTaxaValidator : AbstractValidator<CreateMarketplaceTaxaDto>
{
    public CreateMarketplaceTaxaValidator()
    {
        RuleFor(x => x.ValorInicial)
            .GreaterThanOrEqualTo(0).WithMessage("O valor inicial não pode ser negativo.");

        RuleFor(x => x.ValorFinal)
            .GreaterThanOrEqualTo(0).WithMessage("O valor final não pode ser negativo.");

        RuleFor(x => x.ValorFinal)
            .GreaterThanOrEqualTo(x => x.ValorInicial)
            .When(x => x.ValorInicial.HasValue && x.ValorFinal.HasValue)
            .WithMessage("O valor final deve ser maior ou igual ao valor inicial.");

        RuleFor(x => x.Comissao)
            .InclusiveBetween(0, 100).WithMessage("A comissão deve estar entre 0 e 100.");

        RuleFor(x => x.TaxaFixa)
            .GreaterThanOrEqualTo(0).WithMessage("A taxa fixa não pode ser negativa.");
    }
}

public class UpdateMarketplaceTaxaValidator : AbstractValidator<UpdateMarketplaceTaxaDto>
{
    public UpdateMarketplaceTaxaValidator()
    {
        RuleFor(x => x.IdTaxa)
            .NotEmpty().WithMessage("O ID da taxa é obrigatório.")
            .When(x => x.IdTaxa.HasValue);

        RuleFor(x => x.ValorInicial)
            .GreaterThanOrEqualTo(0).WithMessage("O valor inicial não pode ser negativo.");

        RuleFor(x => x.ValorFinal)
            .GreaterThanOrEqualTo(0).WithMessage("O valor final não pode ser negativo.");

        RuleFor(x => x.ValorFinal)
            .GreaterThanOrEqualTo(x => x.ValorInicial)
            .When(x => x.ValorInicial.HasValue && x.ValorFinal.HasValue)
            .WithMessage("O valor final deve ser maior ou igual ao valor inicial.");

        RuleFor(x => x.Comissao)
            .InclusiveBetween(0, 100).WithMessage("A comissão deve estar entre 0 e 100.");

        RuleFor(x => x.TaxaFixa)
            .GreaterThanOrEqualTo(0).WithMessage("A taxa fixa não pode ser negativa.");
    }
}
