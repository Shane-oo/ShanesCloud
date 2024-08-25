using ShanesCloud.Core;

namespace ShanesCloud.Users.Users;

public class CreateUserHandler: ICommandHandler<CreateUserCommand>
{
    public async Task<Result> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        return Result.Success();
    }
}
