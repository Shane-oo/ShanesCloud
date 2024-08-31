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
    #region Construction

    public UsersApiModule(): base("users")
    {
    }

    #endregion

    #region Public Methods

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (CreateUserRequest request, ISender sender, CancellationToken cancellationToken) =>
                         {
                             var command = new CreateUserCommand(request.Role)
                                           {
                                               UserName = request.UserName,
                                               Email = request.Email,
                                               Password = request.Password
                                           };

                             var result = await sender.Send(command, cancellationToken);

                             return result.IsFailure ? Results.BadRequest(result.ErrorResult) : Results.Created();
                         });
    }

    #endregion
}
