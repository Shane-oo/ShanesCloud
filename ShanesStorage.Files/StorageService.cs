using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace ShanesStorage.Files;

public interface IStorageService
{
    public Task<List<string>> GetContainers(CancellationToken cancellationToken);
}

public class StorageService: IStorageService
{
    private readonly BlobServiceClient _blobServiceClient;

    public StorageService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<List<string>> GetContainers(CancellationToken cancellationToken)
    {
        var list = new List<string>();

        await foreach(var container in _blobServiceClient.GetBlobContainersAsync(BlobContainerTraits.None,
                                                                                 BlobContainerStates.None,
                                                                                 null,
                                                                                 cancellationToken))
        {
            Console.WriteLine(container.Name);
        }

        return list;
    }
}
