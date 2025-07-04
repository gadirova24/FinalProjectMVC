using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using Domain.Entities;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using Org.BouncyCastle.Asn1.Ocsp;
using Service.Helpers.Enums;
using Service.Helpers.Exceptions;

using Service.Services.Interfaces;
using Service.ViewModels.Admin.Account;
using Service.ViewModels.UI;

namespace Service.Services
{
	public class AccountService:IAccountService
	{
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;
        public AccountService(UserManager<AppUser> userManager,
                              SignInManager<AppUser> signInManager,
                               RoleManager<IdentityRole> roleManager,
                               IEmailService emailService,
                               IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _config = config;
        }

        public async Task CreateRolesAsync()
        {
            foreach (var role in Enum.GetValues(typeof(Roles)))
            {
                if (!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
                }
            }


        }


        public async Task LoginAsync(LoginVM model)
        {
            AppUser user = await _userManager.FindByEmailAsync(model.UsernameOrEmail);
            if (user is null)
            {
                user = await _userManager.FindByNameAsync(model.UsernameOrEmail);
            }
            if (user is null)
            {
                throw new NotFoundException("User not found");
            }
            bool result = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!result)
            {
                throw new ValidationException("USername or password is wrong");
            }
            if (!await _userManager.IsEmailConfirmedAsync(user))
                throw new ValidationException("Please confirm your email");
            await _signInManager.SignInAsync(user, false);

        }




        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> RegisterAsync(RegisterVM model, string appUrl)
        {
            var user = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                var confirmationLink = $"{appUrl}/Account/ConfirmEmail?userId={user.Id}&token={encodedToken}";

                var message = $"Please confirm your email by clicking <a href='{confirmationLink}'>here</a>.";

                await _emailService.SendEmailAsync(user.Email, "Confirm your email", message);
                await _userManager.AddToRoleAsync(user, Roles.Member.ToString());
            }
          
            return result;
        }


        public async Task ForgotPasswordAsync(string email, string appUrl)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                throw new NotFoundException("User not found or email not confirmed.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var resetLink = $"{appUrl}/Account/ResetPassword?userId={user.Id}&token={encodedToken}";
            var body = $@"
        <p>Hello <strong>{user.UserName}</strong>,</p>
        <p>Click <a href='{resetLink}'>here</a> to reset your password.</p>";

            await _emailService.SendEmailAsync(user.Email, "Reset your password", body);
        }

        public async Task<IdentityResult> ResetPasswordAsync(string userId, string token, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new NotFoundException("User not found");

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            return await _userManager.ResetPasswordAsync(user, decodedToken, newPassword);
        }

        public async Task<IEnumerable<UserVM>> GetAllUsersAsync()
        {
            var users = _userManager.Users.ToList();
            var datas = new List<UserVM>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                datas.Add(new UserVM
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    UserName = user.UserName,
                    Roles = roles.ToArray()
                });
            }

            return datas;
        }
        public async Task<IEnumerable<RoleVM>> GetAllRolesAsync()
        {
            var roles = _roleManager.Roles.ToList();
            var datas = new List<RoleVM>();

            foreach (var role in roles)
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
                var userDatas = usersInRole.Select(u => new UserVM
                {

                    FullName = u.UserName, 
                    Email = u.Email,
                    UserName = u.UserName,
                    Roles = new[] { role.Name }
                }).ToArray();

                datas.Add(new RoleVM
                {
                    Name = role.Name,
                    Users = userDatas
                });
            }

            return datas;
        }
        public async Task AddRoleToUserAsync(UserRoleVM model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) throw new Exception("User not found ");

            if (await _userManager.IsInRoleAsync(user, model.RoleName))
                throw new Exception("User has already this role");

            var result = await _userManager.AddToRoleAsync(user, model.RoleName);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Cannot add role: {errors}");
            }
        }

        public async Task RemoveRoleFromUserAsync(UserRoleVM model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) throw new Exception("User not found");

            if (model.RoleName == Roles.Member.ToString())
                throw new Exception(" Cannot delete role Member .");

            if (!await _userManager.IsInRoleAsync(user, model.RoleName))
                throw new Exception("user has not this role.");

            var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Cannot delete this role: {errors}");
            }
            if (!await _userManager.IsInRoleAsync(user, Roles.Member.ToString()))
            {
                await _userManager.AddToRoleAsync(user, Roles.Member.ToString());
            }
        }

    }
}

