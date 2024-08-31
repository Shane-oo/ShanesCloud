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
using ShanesCloud.Core;
using OpenIddictErrors = OpenIddict.Abstractions.OpenIddictConstants.Errors;
using OpenIddictClaims = OpenIddict.Abstractions.OpenIddictConstants.Claims;
using OpenIddictDestinations = OpenIddict.Abstractions.OpenIddictConstants.Destinations;
using OpenIddictScopes = OpenIddict.Abstractions.OpenIddictConstants.Scopes;

namespace ShanesCloud.Auth;

[UsedImplicitly]
public class AuthApiModule: CarterModule
{
    private const string AUTH_SCHEME = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme;

    #region Construction

    public AuthApiModule(): base("auth")
    {
    }

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

    #endregion

    #region Public Methods

    private static IResult SignInUser(OpenIddictRequest request, UserAuthenticationPayload payload)
    {
        var identity = new ClaimsIdentity(AUTH_SCHEME, OpenIddictClaims.Username, OpenIddictClaims.Role);

        identity.AddClaim(OpenIddictClaims.ClientId, request.ClientId!);
        identity.AddClaim(OpenIddictClaims.Subject, payload.UserId.Value.ToString());
        identity.AddClaim(OpenIddictClaims.Username, payload.Username);
        identity.AddClaim(OpenIddictClaims.Role, payload.Role.ToString());
        
        // allow all claims to be added in the access tokens
        identity.SetDestinations(_ => [OpenIddictDestinations.AccessToken]);
        
        var principal = new ClaimsPrincipal(identity);
        
        principal.SetScopes(OpenIddictScopes.OfflineAccess, OpenIddictScopes.Roles);
        
        return Results.SignIn(principal, null, AUTH_SCHEME);
    }
    
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
                                      var command = new AuthenticateUserWithPasswordCommand
                                                    {
                                                        Email = request.Username,
                                                        Password = request.Password
                                                    };
                                      var result = await sender.Send(command, cancellationToken);

                                      return result.IsFailure ? Forbid(result.ErrorResult) : SignInUser(request, result.Value);
                                  }

                                  return Forbid(new Error(OpenIddictErrors.UnsupportedGrantType, "Grant type not supported"));
                              }
                   );
        
        app.MapGet("/fakeData", async (IOpenIddictApplicationManager manager) =>
                                {
                                    var application = new OpenIddictApplicationDescriptor
                                                      {
                                                          ClientId = "ShanesCloud",
                                                          DisplayName = "ShanesCloud Public Client Application",
                                                          Permissions =
                                                          {
                                                              OpenIddictConstants.Permissions.Endpoints.Token,

                                                              OpenIddictConstants.Permissions.GrantTypes.Password,
                                                              OpenIddictConstants.Permissions.GrantTypes.RefreshToken,

                                                              OpenIddictConstants.Permissions.ResponseTypes.Token
                                                          },
                                                          ClientType = OpenIddictConstants.ClientTypes.Public
                                                      };
                                    await manager.CreateAsync(application);
                                });
    }

    #endregion
}
