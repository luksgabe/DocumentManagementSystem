namespace DocumentManagement.Core.Models;

public sealed class UserModel
{
    public string Id { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Role { get; set; } = default!;
}
