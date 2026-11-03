using AppleAcademy.Application.Dtos;
using FluentValidation;

namespace AppleAcademy.Application.Validators;

public class CreateStatusValidator: AbstractValidator<StatusCreateDto>
{
    public CreateStatusValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Descrição é obrigatória.")
            .MinimumLength(3).WithMessage("Descrição deve ter no mínimo 3 caracteres.")
            .MaximumLength(50).WithMessage("Descrição não deve exceder 50 caracteres.");
    }
}
