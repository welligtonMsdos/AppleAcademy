using AppleAcademy.Application.Dtos;
using FluentValidation;

namespace AppleAcademy.Application.Validators;

public class UpdateUserValidator : AbstractValidator<UserUpdateDto>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MinimumLength(3).WithMessage("Nome deve ter no mínimo 3 caracteres.")
            .MaximumLength(150).WithMessage("Nome não deve exceder 150 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório.")
            .MinimumLength(13).WithMessage("Email deve ter no mínimo 13 caracteres.")
            .MaximumLength(150).WithMessage("Email não deve exceder 150 caracteres.")
            .EmailAddress().WithMessage("Email inválido.");        

        RuleFor(x => x.PerfilGuid)
            .NotEmpty().WithMessage("Perfil é obrigatório.");
    }
}
