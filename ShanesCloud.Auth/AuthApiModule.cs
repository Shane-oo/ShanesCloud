using System.Security.Claims;
using Carter;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using ShanesCloud.Auth.Authentications;
using ShanesCloud.Auth.Authentications.AuthenticateUserWithRefreshToken;
using ShanesCloud.Core;
using ShanesCloud.Data.Entities;
using OpenIddictErrors = OpenIddict.Abstractions.OpenIddictConstants.Errors;
using OpenIddictClaims = OpenIddict.Abstractions.OpenIddictConstants.Claims;
using OpenIddictDestinations = OpenIddict.Abstractions.OpenIddictConstants.Destinations;
using OpenIddictScopes = OpenIddict.Abstractions.OpenIddictConstants.Scopes;

namespace ShanesCloud.Auth;

[UsedImplicitly]
public class AuthApiModule: CarterModule
{
    #region Constants

    private const string AUTH_SCHEME = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme;

    #endregion

    #region Construction

    public AuthApiModule(): base("auth")
    {
    }

    #endregion

    #region Private Methods

    private static IResult Forbid(Error error)
    {
        return Results.Forbid(new AuthenticationProperties(new Dictionary<string, string>
                                                           {
                                                               [OpenIddictServerAspNetCoreConstants.Properties.Error] = error.Code,
                                                               [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                                                                   error.Description
                                                           }),
                              new[] { AUTH_SCHEME });
    }

    private static async Task<IResult> LoginWithPassword(OpenIddictRequest request, ISender sender, CancellationToken cancellationToken)
    {
        var command = new AuthenticateUserWithPasswordCommand
                      {
                          Email = request.Username,
                          Password = request.Password
                      };
        var result = await sender.Send(command, cancellationToken);

        return result.IsFailure ? Forbid(result.ErrorResult) : SignInUser(request, result.Value);
    }

    private static async Task<IResult> RefreshToken(ClaimsPrincipal principal, ISender sender, CancellationToken cancellationToken)
    {
        var subject = principal.GetClaim(OpenIddictClaims.Subject);
        if (string.IsNullOrEmpty(subject) || !Guid.TryParse(subject, out var userId))
        {
            return Forbid(new Error(OpenIddictErrors.InvalidRequestObject, "Missing required subject claim"));
        }

        var command = new AuthenticateUserWithRefreshTokenCommand(new UserId(userId));
        var result = await sender.Send(command, cancellationToken);

        return result.IsFailure ? Forbid(result.ErrorResult) : Results.SignIn(principal, null, AUTH_SCHEME);
    }

    private static IResult SignInUser(OpenIddictRequest request, UserAuthenticationPayload payload)
    {
        var identity = new ClaimsIdentity(AUTH_SCHEME, OpenIddictClaims.Username, OpenIddictClaims.Role);

        identity.AddClaim(OpenIddictClaims.ClientId, request.ClientId!);
        identity.AddClaim(OpenIddictClaims.Subject, payload.UserId.Value.ToString());
        identity.AddClaim(OpenIddictClaims.Username, payload.Username);
        identity.AddClaim(OpenIddictClaims.Role, payload.Role.ToString());

        // allow all claims to be added in the tokens
        identity.SetDestinations(_ => [OpenIddictDestinations.AccessToken, OpenIddictDestinations.IdentityToken]);

        var principal = new ClaimsPrincipal(identity);

        principal.SetScopes(OpenIddictScopes.OfflineAccess, OpenIddictScopes.OpenId, OpenIddictScopes.Roles, OpenIddictScopes.Profile);

        return Results.SignIn(principal, null, AUTH_SCHEME);
    }

    #endregion

    #region Public Methods

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/token", async (HttpContext httpContext, ISender sender, CancellationToken cancellationToken) =>
                              {
                                  var request = httpContext.GetOpenIddictServerRequest();

                                  if (request == null)
                                  {
                                      return Forbid(new Error(OpenIddictErrors.InvalidRequestObject, "The token request could not be retrieved"));
                                  }

                                  if (request.IsPasswordGrantType())
                                  {
                                      return await LoginWithPassword(request, sender, cancellationToken);
                                  }

                                  if (request.IsRefreshTokenGrantType())
                                  {
                                      // Retrieve the claims principal stored in the refresh token
                                      var principal = (await httpContext.AuthenticateAsync(AUTH_SCHEME)).Principal;
                                      if (principal == null)
                                      {
                                          return Forbid(new Error(OpenIddictErrors.InvalidRequest, "Could not get principal from refresh token"));
                                      }

                                      return await RefreshToken(principal, sender, cancellationToken);
                                  }

                                  return Forbid(new Error(OpenIddictErrors.UnsupportedGrantType, "Grant type not supported"));
                              }
                   );
    }

    #endregion
}
