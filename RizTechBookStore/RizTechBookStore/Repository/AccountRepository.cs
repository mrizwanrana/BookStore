using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using RizTechBookStore.Models;
using RizTechBookStore.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RizTechBookStore.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public AccountRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, 
            IUserService userService, IEmailService emailService, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
        public async Task<IdentityResult> CreateUserAsync(SignUpUserModel signUpUserModel)
        {
            var user = new ApplicationUser
            {
                FirstName = signUpUserModel.FirstName,
                LastName = signUpUserModel.LastName,
                Email = signUpUserModel.Email,
                UserName = signUpUserModel.Email
            };
            var result = await _userManager.CreateAsync(user, signUpUserModel.Password);
            if (result.Succeeded)
            {
                await GenerateEmailConfirmationTokenAsync(user);
            }
            return result;
        }

        public async Task GenerateEmailConfirmationTokenAsync(ApplicationUser applicationUser)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
            if (!string.IsNullOrEmpty(token))
            {
                await SendConfirmationEmail(applicationUser, token);
            }
        }

        public async Task GenerateForgotPasswordTokenAsync(ApplicationUser applicationUser)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(applicationUser);
            if (!string.IsNullOrEmpty(token))
            {
                await SendForgotPasswordEmail(applicationUser, token);
            }
        }
        public async Task<SignInResult> PasswordSignInAsync(SignInModel signInModel)
        {
            var result = await _signInManager.PasswordSignInAsync(signInModel.Email, signInModel.Password, signInModel.RememberMe, false);
            return result;
        }

        public async Task SignOutAsync()
        {
           await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel changePasswordModel)
        {
            var userId = _userService.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            return await _userManager.ChangePasswordAsync(user, changePasswordModel.CurrentPassword, changePasswordModel.NewPassword);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string uid, string token)
        {
            return await _userManager.ConfirmEmailAsync(await _userManager.FindByIdAsync(uid), token);
        }

        public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordModel resetPasswordModel)
        {
            var user = await _userManager.FindByIdAsync(resetPasswordModel.UserId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Invalid user." });
            }

            return await _userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.NewPassword);
        }

        private async Task SendConfirmationEmail(ApplicationUser applicationUser, string token)
        {
            string appDomain = _configuration.GetSection("Application:AppDomain").Value;
            string confirmationLink = _configuration.GetSection("Application:EmailConfirmation").Value;
            UserEmailOptions userEmailOptions = new UserEmailOptions
            {
                ToEmails = new List<string>() { applicationUser.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}", applicationUser.FirstName),
                    // URL-encode token so it is safe in query string
                    new KeyValuePair<string, string>("{{Link}}", string.Format(appDomain + confirmationLink, applicationUser.Id, System.Net.WebUtility.UrlEncode(token)))
                }
            };
            await _emailService.SendEmailForConfirmation(userEmailOptions);
        }

        private async Task SendForgotPasswordEmail(ApplicationUser applicationUser, string token)
        {
            string appDomain = _configuration.GetSection("Application:AppDomain").Value;
            string confirmationLink = _configuration.GetSection("Application:ForgotPassword").Value;
            UserEmailOptions userEmailOptions = new UserEmailOptions
            {
                ToEmails = new List<string>() { applicationUser.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}", applicationUser.FirstName),
                    // URL-encode token so it is safe in query string
                    new KeyValuePair<string, string>("{{Link}}", string.Format(appDomain + confirmationLink, applicationUser.Id, System.Net.WebUtility.UrlEncode(token)))
                }
            };
            await _emailService.SendEmailForgotPassword(userEmailOptions);
        }
    }
}
