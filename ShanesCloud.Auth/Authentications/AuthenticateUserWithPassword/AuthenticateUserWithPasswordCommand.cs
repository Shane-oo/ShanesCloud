using FluentValidation;
using JetBrains.Annotations;
using ShanesCloud.Core;

namespace ShanesCloud.Auth.Authentications;

public class AuthenticateUserWithPasswordCommand: Command<UserAuthenticationPayload>
{
    #region Properties

    public required string Email { get; init; }

    public required string Password { get; init; }

    #endregion
}

[UsedImplicitly]
public class AuthenticateUserWithPasswordCommandValidator: AbstractValidator<AuthenticateUserWithPasswordCommand>
{
    #region Construction

    public AuthenticateUserWithPasswordCommandValidator()
    {
        RuleFor(c => c.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(c => c.Password)
            .NotEmpty();
    }

    #endregion
}
