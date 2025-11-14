using DocumentManagement.Core.Models;

namespace DocumentManagement.Core.Interfaces.Repositories;

public interface IAuthRepository
{
    Task<UserModel?> ValidateUserAsync(string email, string password);
}
