using AppleAcademy.Domain.Entities;

namespace AppleAcademy.Domain.Interfaces;

public interface IPerfilRepository: IRead<Perfil>, ICommand<Perfil>
{
}
