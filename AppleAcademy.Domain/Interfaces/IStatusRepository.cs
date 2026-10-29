using AppleAcademy.Domain.Entities;

namespace AppleAcademy.Domain.Interfaces;

public interface IStatusRepository: IRead<Status>, ICommand<Status>
{
}
