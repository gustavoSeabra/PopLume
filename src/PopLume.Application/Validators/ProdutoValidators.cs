using FluentValidation;
using PopLume.Application.Dtos;

namespace PopLume.Application.Validators;

public class CreateProdutoValidator : AbstractValidator<CreateProdutoDto>
{
    public CreateProdutoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome do produto é obrigatório.");

        RuleFor(x => x.PrecoCusto)
            .GreaterThanOrEqualTo(0).WithMessage("O preço de custo não pode ser negativo.");

        RuleFor(x => x.QuantidadeFilamento)
            .GreaterThanOrEqualTo(0).WithMessage("A quantidade de filamento não pode ser negativa.");

        RuleFor(x => x.TempoImpressao)
            .GreaterThanOrEqualTo(0).WithMessage("O tempo de impressão não pode ser negativo.");
    }
}

public class UpdateProdutoValidator : AbstractValidator<UpdateProdutoDto>
{
    public UpdateProdutoValidator()
    {
        RuleFor(x => x.IdProduto)
            .NotEmpty().WithMessage("O ID do produto é obrigatório.");

        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome do produto é obrigatório.");

        RuleFor(x => x.PrecoCusto)
            .GreaterThanOrEqualTo(0).WithMessage("O preço de custo não pode ser negativo.");

        RuleFor(x => x.QuantidadeFilamento)
            .GreaterThanOrEqualTo(0).WithMessage("A quantidade de filamento não pode ser negativa.");

        RuleFor(x => x.TempoImpressao)
            .GreaterThanOrEqualTo(0).WithMessage("O tempo de impressão não pode ser negativo.");
    }
}