using Microsoft.AspNetCore.Mvc;
using Travel.Context.Entities;

namespace Travel.Services.UserAccount;
public interface IUserAccountService
{
    Task<bool> IsEmpty();

    /// <summary>
    /// Create user account
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<UserAccountModel> Create(RegisterUserAccountModel model);
    Task<bool> Exists(string email);
    Task<bool> ChangeUserStatus(Guid userId, UserStatus newStatus);
    Task<UserStatus> GetStatus(Guid userId);
    Task<Guid> GetUserIdByEmail(String email);
    Task<bool> Delete(string email);
}