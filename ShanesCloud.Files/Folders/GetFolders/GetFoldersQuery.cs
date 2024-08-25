﻿using ShanesCloud.Core.Exchanges;

namespace ShanesCloud.Files.Folders;

public class GetFoldersQuery: Query<List<FolderListModel>>
{
    public string ParentFolder { get; set; }
}