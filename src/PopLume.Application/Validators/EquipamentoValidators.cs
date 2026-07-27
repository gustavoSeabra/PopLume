using FluentValidation;
using PopLume.Application.Dtos;

namespace PopLume.Application.Validators;

public class CreateEquipamentoValidator : AbstractValidator<CreateEquipamentoDto>
{
    public CreateEquipamentoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome do equipamento é obrigatório.")
            .MaximumLength(100).WithMessage("O nome do equipamento deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Apelido)
            .MaximumLength(50).WithMessage("O apelido do equipamento deve ter no máximo 50 caracteres.");

        RuleFor(x => x.DataCompra)
            .NotEmpty().WithMessage("A data de compra é obrigatória.");

        RuleFor(x => x.Potencia)
            .GreaterThanOrEqualTo(0).WithMessage("A potência não pode ser negativa.");

        RuleFor(x => x.ValorCompra)
            .GreaterThanOrEqualTo(0).WithMessage("O valor de compra não pode ser negativo.");

        RuleFor(x => x.ExpectativaVida)
            .GreaterThan(0).WithMessage("A expectativa de vida em horas deve ser maior que zero.");
    }
}

public class UpdateEquipamentoValidator : AbstractValidator<UpdateEquipamentoDto>
{
    public UpdateEquipamentoValidator()
    {
        RuleFor(x => x.IdEquipamento)
            .NotEmpty().WithMessage("O ID do equipamento é obrigatório.");

        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome do equipamento é obrigatório.")
            .MaximumLength(100).WithMessage("O nome do equipamento deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Apelido)
            .MaximumLength(50).WithMessage("O apelido do equipamento deve ter no máximo 50 caracteres.");

        RuleFor(x => x.Potencia)
            .GreaterThanOrEqualTo(0).WithMessage("A potência não pode ser negativa.");

        RuleFor(x => x.ValorCompra)
            .GreaterThanOrEqualTo(0).WithMessage("O valor de compra não pode ser negativo.");

        RuleFor(x => x.ExpectativaVida)
            .GreaterThan(0).WithMessage("A expectativa de vida em horas deve ser maior que zero.");
    }
}
