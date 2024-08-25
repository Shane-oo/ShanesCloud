using JetBrains.Annotations;
using ShanesCloud.Core.Exchanges;

namespace ShanesCloud.Files.Folders;

[UsedImplicitly]
public class GetFoldersHandler: IQueryHandler<GetFoldersQuery, List<FolderListModel>>
{
    private readonly IStorageService _storageService;

    public GetFoldersHandler(IStorageService storageService)
    {
        _storageService = storageService;
    }

    #region Public Methods

    public async Task<Result<List<FolderListModel>>> Handle(GetFoldersQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        if (string.IsNullOrEmpty(query.ParentFolder))
        {
            // we'll be wanting the containers if no parent folder is give (top level)

            // query database!
        }
        else
        {
            // well be wanting to find the children of the parent folder given!

            // query database

            // interaction with storage will only be for creating new containers/child docs 
            // and retrieving files
        }

        return new List<FolderListModel>();
    }

    #endregion
}
