using System.ComponentModel.DataAnnotations;

namespace ShanesCloud.Api;

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
