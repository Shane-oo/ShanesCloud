using MediatR;
using ShanesCloud.Data.Entities;

namespace ShanesCloud.Core;

public interface IBaseCommand
{
    public UserId UserId { get; }
}

public interface ICommand: IRequest<Result>, IBaseCommand;

public interface ICommand<TResponse>: IRequest<Result<TResponse>>, IBaseCommand;

public class Command<TResponse>: ICommand<TResponse>
{
    #region Properties

    public UserId UserId { get; }

    #endregion

    #region Construction

    public Command(UserId userId)
    {
        UserId = userId;
    }

    public Command()
    {
    }

    #endregion
}

public class Command: ICommand
{
    #region Properties

    public UserId UserId { get; }

    #endregion

    #region Construction

    public Command(UserId userId)
    {
        UserId = userId;
    }

    public Command()
    {
    }

    #endregion
}
