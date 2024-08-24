using JetBrains.Annotations;
using ShanesStorage.Core.Exchanges;

namespace ShanesStorage.Files.Folders;

[UsedImplicitly]
public class GetFoldersHandler: IQueryHandler<GetFoldersQuery, List<FolderListModel>>
{
    #region Public Methods

    public async Task<Result<List<FolderListModel>>> Handle(GetFoldersQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);


        return new List<FolderListModel>();
    }

    #endregion
}
