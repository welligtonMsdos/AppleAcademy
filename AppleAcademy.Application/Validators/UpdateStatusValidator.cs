using AppleAcademy.Application.Dtos;
using FluentValidation;

namespace AppleAcademy.Application.Validators;

public class UpdateStatusValidator : AbstractValidator<StatusUpdateDto>
{
    public UpdateStatusValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Descrição é obrigatória.")
            .MinimumLength(8).WithMessage("Descrição deve ter no mínimo 8 caracteres.")
            .MaximumLength(50).WithMessage("Descrição não deve exceder 50 caracteres.");
    }
}
