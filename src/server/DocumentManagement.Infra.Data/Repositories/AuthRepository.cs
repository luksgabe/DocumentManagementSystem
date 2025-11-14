using DocumentManagement.Core.Interfaces.Repositories;
using DocumentManagement.Core.Models;

namespace DocumentManagement.Infra.Data.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly List<UserModel> _users = new()
    {
        new UserModel { Id = "1", Email = "admin@company.com", Password = "Admin@123", Role = "Admin" },
        new UserModel { Id = "2", Email = "manager@company.com", Password = "Manager@123", Role = "Manager" },
        new UserModel { Id = "3", Email = "user@company.com", Password = "User@123", Role = "Viewer" },
    };

    public Task<UserModel?> ValidateUserAsync(string email, string password)
    {
        var user = _users.FirstOrDefault(u =>
            u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) &&
            u.Password == password);

        return Task.FromResult(user);
    }


}
