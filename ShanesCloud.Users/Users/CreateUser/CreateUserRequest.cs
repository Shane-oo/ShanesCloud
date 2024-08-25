namespace ShanesCloud.Users.Users;

public class CreateUserRequest
{
    public string Email { get; set; }

    public string Password { get; set; }

    public NewRoleRequest RoleRequest { get; set; }

    public string UserName { get; set; }
}

public enum NewRoleRequest: byte
{
    None = 0,
    Admin = 1,
    User = 2,
    Guest = 3
}
