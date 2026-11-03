using AppleAcademy.Application.Dtos;
using FluentValidation;

namespace AppleAcademy.Application.Validators;

public class UpdatePerfilValidator : AbstractValidator<PerfilUpdateDto>
{
    public UpdatePerfilValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MinimumLength(5).WithMessage("Nome deve ter no mínimo 5 caracteres.")
            .MaximumLength(50).WithMessage("Nome não deve exceder 50 caracteres.");
    }
}
