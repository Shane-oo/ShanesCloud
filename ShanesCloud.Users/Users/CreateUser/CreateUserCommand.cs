using FluentValidation;
using JetBrains.Annotations;
using ShanesCloud.Core;
using ShanesCloud.Data.Entities;

namespace ShanesCloud.Users.Users;

public class CreateUserCommand: Command
{
    #region Properties

    public string Email { get; set; }

    public string Password { get; set; }

    public Roles Role { get; set; }

    public string UserName { get; set; }

    #endregion

    #region Construction

    public CreateUserCommand(NewRoleRequest newRoleRequest)
    {
        Role = newRoleRequest switch
               {
                   NewRoleRequest.Admin => Roles.Admin,
                   NewRoleRequest.User => Roles.User,
                   NewRoleRequest.Guest => Roles.Guest,
                   _ => Roles.None
               };
    }

    #endregion
}

[UsedImplicitly]
public class CreateUserCommandValidator: AbstractValidator<CreateUserCommand>
{
    #region Construction

    public CreateUserCommandValidator()
    {
        RuleFor(c => c.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(c => c.UserName)
            .NotEmpty()
            .MaximumLength(30);
        RuleFor(c => c.Role)
            .IsInEnum()
            .Must(c => c != Roles.None)
            .WithMessage($"Role must be {nameof(Roles.Admin)} or {nameof(Roles.User)} or {nameof(Roles.Guest)}");
        RuleFor(c => c.Password)
            .NotEmpty()
            .MinimumLength(12)
            .When(c => c.Role is Roles.Admin or Roles.User)
            .WithMessage("Password must be set with 12 characters when role is Admin or User");
        RuleFor(c => c.Password)
            .Empty()
            .When(c => c.Role is Roles.Guest)
            .WithMessage("Password must be empty when role is Guest");
    }

    #endregion
}
