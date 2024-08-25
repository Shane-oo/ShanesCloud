using Carter;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ShanesCloud.Users.Users;

namespace ShanesCloud.Users;

[UsedImplicitly]
public class UsersApiModule: CarterModule
{
    #region Fields

    private readonly ISender _sender;

    #endregion

    #region Construction

    public UsersApiModule(ISender sender): base("users")
    {
        _sender = sender;
    }

    #endregion

    #region Public Methods

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (CreateUserRequest request, CancellationToken cancellationToken) =>
                         {
                             var command = new CreateUserCommand(request.RoleRequest)
                                           {
                                               UserName = request.UserName,
                                               Email = request.Email,
                                               Password = request.Password
                                           };

                             var result = await _sender.Send(command, cancellationToken);

                             return result.IsFailure ? Results.BadRequest(result.ErrorResult) : Results.Created();
                         });
    }

    #endregion
}
