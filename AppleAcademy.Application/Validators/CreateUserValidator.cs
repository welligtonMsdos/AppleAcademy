using AppleAcademy.Application.Dtos;
using FluentValidation;

namespace AppleAcademy.Application.Validators;

public class CreateUserValidator : AbstractValidator<UserCreateDto>
{
    public CreateUserValidator()
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

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Senha é obrigatória.")
            .MinimumLength(10).WithMessage("Senha deve ter no mínimo 10 caracteres.")
            .MaximumLength(100).WithMessage("Senha não deve exceder 100 caracteres.");

        RuleFor(x => x.PerfilGuid)
            .NotEmpty().WithMessage("Perfil é obrigatório.");
    }
}
