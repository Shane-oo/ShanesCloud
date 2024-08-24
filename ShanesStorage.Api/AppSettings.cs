using System.ComponentModel.DataAnnotations;

namespace ShanesStorage.Api;

public class AppSettings
{
    [Required]
    public StorageAccountSettings StorageAccountSettings { get; set; }
}

public class StorageAccountSettings
{
    [Required]
    public string Connection { get; set; }
}
