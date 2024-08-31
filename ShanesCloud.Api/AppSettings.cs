using System.ComponentModel.DataAnnotations;

namespace ShanesCloud.Api;

public class AppSettings
{
    #region Properties

    [Required]
    public StorageAccountSettings StorageAccountSettings { get; set; }
    
    [Required]
    public AuthServerSettings AuthServerSettings { get; set; }

    #endregion
}

public class StorageAccountSettings
{
    #region Properties

    [Required]
    public string Connection { get; set; }

    #endregion
}

public class AuthServerSettings
{
    #region Properties

    [Required]
    public string EncryptionCertificateThumbprint { get; set; }

    [Required]
    public string SigningCertificateThumbprint { get; set; }

    #endregion
}
