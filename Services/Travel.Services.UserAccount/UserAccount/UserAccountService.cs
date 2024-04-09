namespace Travel.Services.UserAccount;

using AutoMapper;
using Travel.Common.Exceptions;
using Travel.Common.Validator;
using Travel.Context.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Travel.Context;

public class UserAccountService : IUserAccountService
{
    private readonly IDbContextFactory<MainDbContext> dbContextFactory;
    private readonly IMapper mapper;
    private readonly UserManager<User> userManager;
    private readonly IModelValidator<RegisterUserAccountModel> registerUserAccountModelValidator;

    public UserAccountService(
        IDbContextFactory<MainDbContext> dbContextFactory,
        IMapper mapper,
        UserManager<User> userManager,
        IModelValidator<RegisterUserAccountModel> registerUserAccountModelValidator
    )
    {
        this.dbContextFactory = dbContextFactory;
        this.mapper = mapper;
        this.userManager = userManager;
        this.registerUserAccountModelValidator = registerUserAccountModelValidator;
    }

    public async Task<bool> IsEmpty()
    {
        return !(await userManager.Users.AnyAsync());
    }

    public async Task<UserAccountModel> Create(RegisterUserAccountModel model)
    {
        registerUserAccountModelValidator.Check(model);

        // Find user by email
        var user = await userManager.FindByEmailAsync(model.Email);
        if (user != null)
            throw new ProcessException($"User account with email {model.Email} already exist.");

        // Create user account
        user = new User()
        {
            Status = UserStatus.Active,
            FullName = model.Name,
            UserName = model.Email,  // это логин, приравниваем его к email
            Email = model.Email,
            EmailConfirmed = true, // считаем, что почта подтверждена
            PhoneNumber = null,
            PhoneNumberConfirmed = false
        };

        var result = await userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
            throw new ProcessException($"Creating user account is wrong. {string.Join(", ", result.Errors.Select(s => s.Description))}");

        return mapper.Map<UserAccountModel>(user);
    }

    public async Task<bool> Exists(string email)
    {
        using var context = await dbContextFactory.CreateDbContextAsync();
        return await context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<bool> ChangeUserStatus(Guid userId, UserStatus newStatus)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new ProcessException($"User with id {userId} not found.");

        user.Status = newStatus;
        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new ProcessException($"Failed to update user status. {string.Join(", ", result.Errors.Select(s => s.Description))}");

        return true;
    }

    public async Task<UserStatus> GetStatus(Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new ProcessException($"User with id {userId} not found.");

        return user.Status;
    }


}
