using FluentValidation;
using PopLume.Application.Dtos;

namespace PopLume.Application.Validators;

public class CreateFilamentoValidator : AbstractValidator<CreateFilamentoDto>
{
    public CreateFilamentoValidator()
    {
        RuleFor(x => x.Cor)
            .NotEmpty().WithMessage("A cor do filamento é obrigatória.")
            .MaximumLength(50).WithMessage("A cor do filamento deve ter no máximo 50 caracteres.");

        RuleFor(x => x.Valor)
            .NotNull().WithMessage("O valor do filamento é obrigatório.")
            .GreaterThanOrEqualTo(0).WithMessage("O valor do filamento não pode ser negativo.");

        RuleFor(x => x.Peso)
            .NotNull().WithMessage("O peso do filamento é obrigatório.")
            .GreaterThan(0).WithMessage("O peso do filamento em gramas deve ser maior que zero.");

        RuleFor(x => x.Tipo)
            .NotNull().WithMessage("O tipo do filamento é obrigatório.")
            .IsInEnum().WithMessage("O tipo do filamento deve ser ABS, PETG, PLA ou TPU.");

        RuleFor(x => x.DataCompra)
            .NotNull().WithMessage("A data de compra do filamento é obrigatória.");
    }
}

public class UpdateFilamentoValidator : AbstractValidator<UpdateFilamentoDto>
{
    public UpdateFilamentoValidator()
    {
        RuleFor(x => x.IdFilamento)
            .NotEmpty().WithMessage("O ID do filamento é obrigatório.");

        RuleFor(x => x.Cor)
            .NotEmpty().WithMessage("A cor do filamento é obrigatória.")
            .MaximumLength(50).WithMessage("A cor do filamento deve ter no máximo 50 caracteres.");

        RuleFor(x => x.Valor)
            .NotNull().WithMessage("O valor do filamento é obrigatório.")
            .GreaterThanOrEqualTo(0).WithMessage("O valor do filamento não pode ser negativo.");

        RuleFor(x => x.Peso)
            .NotNull().WithMessage("O peso do filamento é obrigatório.")
            .GreaterThan(0).WithMessage("O peso do filamento em gramas deve ser maior que zero.");

        RuleFor(x => x.Tipo)
            .NotNull().WithMessage("O tipo do filamento é obrigatório.")
            .IsInEnum().WithMessage("O tipo do filamento deve ser ABS, PETG, PLA ou TPU.");

        RuleFor(x => x.DataCompra)
            .NotNull().WithMessage("A data de compra do filamento é obrigatória.");
    }
}
