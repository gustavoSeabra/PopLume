using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using PopLume.Application.Dtos;
using PopLume.Application.Validators;

namespace PopLume.Api.Extensions;

public static class ValidatorConfigurationExtensions
{
    public static IServiceCollection AddValidationConfiguration(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();

        services.AddValidatorsFromAssemblyContaining<CreateProdutoValidator>();

        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                string[] errors = ObterErros(context);

                var response = ResultadoDto<bool>.RetornaErro("Erro de validação", errors);

                return new BadRequestObjectResult(response);
            };
        });

        return services;
    }

    private static string[] ObterErros(ActionContext context) => context.ModelState
        .Where(ms => ms.Value?.Errors.Count > 0)
        .SelectMany(ms => ms.Value!.Errors.Select(e => e.ErrorMessage))
        .ToArray();
}
