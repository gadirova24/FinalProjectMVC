using System;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.ViewModels.Admin.Account;
using Service.ViewModels.UI;

namespace Service.Services.Interfaces
{
	public interface IAccountService
	{
        Task<IdentityResult> RegisterAsync(RegisterVM model, string appUrl);
        Task LoginAsync(LoginVM model);
        Task CreateRolesAsync();
        Task LogoutAsync();
        Task ForgotPasswordAsync(string email, string appUrl);
        Task<IdentityResult> ResetPasswordAsync(string userId, string token, string newPassword);
        Task<IEnumerable<UserVM>> GetAllUsersAsync();
        Task<IEnumerable<RoleVM>> GetAllRolesAsync();
        Task AddRoleToUserAsync(UserRoleVM model);
        Task RemoveRoleFromUserAsync(UserRoleVM model);
    }
}

