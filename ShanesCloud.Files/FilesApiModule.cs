using Carter;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ShanesCloud.Files.Folders;

namespace ShanesCloud.Files;

[UsedImplicitly]
public class FilesApiModule: CarterModule
{
    private readonly ISender _sender;

    public FilesApiModule(ISender sender): base("files")
    {
        _sender = sender;
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/list", async ([AsParameters] GetFoldersQueryRequest request, CancellationToken cancellationToken) =>
                            {
                                var query = new GetFoldersQuery
                                            {
                                                ParentFolder = request.ParentFolder
                                            };

                                var result = await _sender.Send(query, cancellationToken);

                                return result.IsFailure ? Results.BadRequest(result.ErrorResult) : Results.Ok(result.Value);
                            });
        app.MapGet("/", () => { Console.WriteLine("Get Folders"); });
    }
}
